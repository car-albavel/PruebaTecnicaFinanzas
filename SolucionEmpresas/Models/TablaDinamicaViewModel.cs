using System.Collections.Generic;
using System.Web.Mvc;

namespace SolucionEmpresas.Models  
{
    public class TablaDinamicaViewModel
    {
        public List<Dictionary<string, object>> Filas { get; set; }
        public List<string> Columnas { get; set; }

        // Filtros
        public string FiltroNombreEmpresa { get; set; }
        public string FiltroPeriodo { get; set; }

        public List<SelectListItem> Periodos { get; set; }

        // Columnas visibles
        public List<string> ColumnasSeleccionadas { get; set; } = new List<string>();
        public List<string> TodasLasColumnas { get; set; } = new List<string>();
    }


}