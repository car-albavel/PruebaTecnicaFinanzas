using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SolucionEmpresas.Models;
using SolucionEmpresas.Services;

namespace TuAplicacion.Controllers
{
    public class IndicadoresController : Controller
    {

        private readonly TraerIndicadores _service = new TraerIndicadores();

        // /Home/Index?filtroNombreEmpresa=...&filtroPeriodo=...&filtroCuentaId=...
        public ActionResult Index(string filtroNombreEmpresa, string filtroPeriodo, string[] columnasSeleccionadas)
        {
            var filas = _service.ObtenerIndicadoresDinamico(); // o tu método dinámico actual

            // 1) Construir combo de Periodos con formato dd/MM/yyyy
            var periodosBrutos = filas
                .Where(f => f.ContainsKey("Periodo") && f["Periodo"] != null)
                .Select(f => f["Periodo"])
                .Distinct()
                .ToList();

            var periodosUnicos = periodosBrutos
                .Select(p =>
                {
                    DateTime fecha;
                    string value = p.ToString(); // valor original (para filtrar)
                    string text;

                    if (p is DateTime)
                    {
                        fecha = (DateTime)p;
                        text = fecha.ToString("dd/MM/yyyy");
                    }
                    else if (DateTime.TryParse(p.ToString(), out fecha))
                    {
                        text = fecha.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        text = value;
                    }

                    return new { Value = value, Text = text };
                })
                .OrderBy(x =>
                {
                    DateTime f;
                    return DateTime.TryParse(x.Value, out f) ? f : DateTime.MaxValue;
                })
                .ToList();

            var listaPeriodos = new List<SelectListItem>();
            listaPeriodos.Add(new SelectListItem { Value = "", Text = "-- Todos --" });
            listaPeriodos.AddRange(
                periodosUnicos.Select(p => new SelectListItem
                {
                    Value = p.Value,
                    Text = p.Text,
                    Selected = (p.Value == filtroPeriodo)
                })
            );

            // 2) Filtros sobre filas
            IEnumerable<Dictionary<string, object>> query = filas;

            if (!string.IsNullOrWhiteSpace(filtroNombreEmpresa))
            {
                query = query.Where(f =>
                    f.ContainsKey("NombreEmpresa") &&
                    f["NombreEmpresa"] != null &&
                    f["NombreEmpresa"].ToString()
                        .IndexOf(filtroNombreEmpresa, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            if (!string.IsNullOrWhiteSpace(filtroPeriodo))
            {
                query = query.Where(f =>
                    f.ContainsKey("Periodo") &&
                    f["Periodo"] != null &&
                    f["Periodo"].ToString() == filtroPeriodo);
            }

            var filasFiltradas = query.ToList();

            // 3) Columnas (todas y visibles)

            // Ahora: respeta el orden del primer registro y añade las demás al final
            var todasLasColumnas = new List<string>();

            foreach (var fila in filasFiltradas)
            {
                foreach (var col in fila.Keys)
                {
                    if (!todasLasColumnas.Contains(col))
                    {
                        todasLasColumnas.Add(col);
                    }
                }
            }


            // Columnas que el usuario puede activar/desactivar
            var columnasFiltrables = todasLasColumnas
                .Where(c => c != "NombreEmpresa" && c != "Periodo")
                .ToList();

            List<string> columnasVisibles;

            // columnasSeleccionadas solo aplica a las columnas filtrables
            if (columnasSeleccionadas == null || columnasSeleccionadas.Length == 0)
            {
                columnasVisibles = columnasFiltrables;
            }
            else
            {
                columnasVisibles = columnasFiltrables
                    .Where(c => columnasSeleccionadas.Contains(c))
                    .ToList();
            }

            // Añadimos SIEMPRE NombreEmpresa y Periodo al conjunto visible, en el orden correcto
            var columnasFinales = new List<string>();

            foreach (var col in todasLasColumnas)
            {
                if (col == "NombreEmpresa" || col == "Periodo")
                {
                    columnasFinales.Add(col); // siempre
                }
                else if (columnasVisibles.Contains(col))
                {
                    columnasFinales.Add(col); // solo si está seleccionada
                }
            }

            var modelo = new TablaDinamicaViewModel
            {
                Filas = filasFiltradas,
                Columnas = columnasFinales,
                FiltroNombreEmpresa = filtroNombreEmpresa,
                FiltroPeriodo = filtroPeriodo,
                Periodos = listaPeriodos,
                ColumnasSeleccionadas = columnasVisibles,
                TodasLasColumnas = todasLasColumnas
            };

            return View(modelo);
        }
    }
}