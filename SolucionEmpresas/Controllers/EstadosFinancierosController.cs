using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using SolucionEmpresas.Models;

namespace TuAplicacion.Controllers
{
    public class EstadosFinancierosController : Controller
    {
        private dbFinanzasEmpresaEntities db = new dbFinanzasEmpresaEntities();

        // GET: EstadosFinancieros
        public ActionResult Index(int? empresaId)
        {
            var estados = db.EstadosFinancieros.AsQueryable();

            if (empresaId.HasValue)
            {
                estados = estados.Where(e => e.EmpresaId == empresaId);
            }

            var listaEstados = estados.OrderByDescending(e => e.Periodo).ToList();

            var empresas = db.Empresas.Where(e => e.Estado).ToList();
            ViewBag.Empresas = new SelectList(empresas, "EmpresaId", "NombreEmpresa", empresaId);
            ViewBag.EmpresaId = empresaId;

            return View(listaEstados);
        }

        // GET: EstadosFinancieros/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            EstadosFinancieros estado = db.EstadosFinancieros.Find(id);
            if (estado == null)
            {
                return HttpNotFound();
            }

            return View(estado);
        }

        // GET: EstadosFinancieros/Create
        public ActionResult Create(int? empresaId)
        {
            var empresas = db.Empresas.Where(e => e.Estado).ToList();
            ViewBag.Empresas = new SelectList(empresas, "EmpresaId", "NombreEmpresa", empresaId);

            var modelo = new EstadosFinancieros
            {
                FechaCargue = DateTime.Now,
                Fecha = DateTime.Now,
                Periodo = DateTime.Now,
                Activa = true
            };

            return View(modelo);
        }

        // POST: EstadosFinancieros/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EmpresaId,Periodo,FechaEstado,FechaCargue,Activo,ActivoCorriente,PasivoCorriente,ActivoTotal,PasivoTotal,Patrimonio,IngresosOperacionales,UtilidadBruta,UtilidadOperativa,UtilidadNeta")] EstadosFinancieros estado)
        {
            if (ModelState.IsValid)
            {
                estado.FechaCargue = DateTime.Now;
                db.EstadosFinancieros.Add(estado);
                

                IndicadoresCalculados indicadoresCalculados = new IndicadoresCalculados() 
                {
                    CuentaId = estado.EstadoId,
                    Indicador1 = 0, Indicador2 = 0, Indicador3 = 0, Indicador4 = 0, Indicador5 = 0
                };

                db.IndicadoresCalculados.Add(indicadoresCalculados);

                db.SaveChanges();

                db.RecalcularResultadosIndicadores();

                TempData["Mensaje"] = "Estado Financiero creado exitosamente.";
                return RedirectToAction("Index", new { empresaId = estado.EmpresaId });
            }

            var empresas = db.Empresas.Where(e => e.Estado).ToList();
            ViewBag.Empresas = new SelectList(empresas, "EmpresaId", "NombreEmpresa", estado.EmpresaId);
            return View(estado);
        }

        // GET: EstadosFinancieros/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            EstadosFinancieros estado = db.EstadosFinancieros.Find(id);
            if (estado == null)
            {
                return HttpNotFound();
            }

            var empresas = db.Empresas.Where(e => e.Estado).ToList();
            ViewBag.Empresas = new SelectList(empresas, "EmpresaId", "NombreEmpresa", estado.EmpresaId);

            return View(estado);
        }

        // POST: EstadosFinancieros/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EstadoId,EmpresaId,Periodo,FechaEstado,FechaCargue,Activo,ActivoCorriente,PasivoCorriente,ActivoTotal,PasivoTotal,Patrimonio,IngresosOperacionales,UtilidadBruta,UtilidadOperativa,UtilidadNeta")] EstadosFinancieros estado)
        {
            if (ModelState.IsValid)
            {
                var estadoExistente = db.EstadosFinancieros.Find(estado.EstadoId);
                if (estadoExistente != null)
                {
                    estadoExistente.Periodo = estado.Periodo;
                    estadoExistente.Fecha = estado.Fecha;
                    estadoExistente.Activa = estado.Activa;
                    estadoExistente.ActivoCorriente = estado.ActivoCorriente;
                    estadoExistente.PasivoCorriente = estado.PasivoCorriente;
                    estadoExistente.ActivoTotal = estado.ActivoTotal;
                    estadoExistente.PasivoTotal = estado.PasivoTotal;
                    estadoExistente.Patrimonio = estado.Patrimonio;
                    estadoExistente.IngresosOperacionales = estado.IngresosOperacionales;
                    estadoExistente.UtilidadBruta = estado.UtilidadBruta;
                    estadoExistente.UtilidadOperativa = estado.UtilidadOperativa;
                    estadoExistente.UtilidadNeta = estado.UtilidadNeta;

                    db.SaveChanges();
                    TempData["Mensaje"] = "Estado Financiero actualizado exitosamente.";
                }

                return RedirectToAction("Index", new { empresaId = estado.EmpresaId });
            }

            var empresas = db.Empresas.Where(e => e.Estado).ToList();
            ViewBag.Empresas = new SelectList(empresas, "EmpresaId", "NombreEmpresa", estado.EmpresaId);
            return View(estado);
        }

        // GET: EstadosFinancieros/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            EstadosFinancieros estado = db.EstadosFinancieros.Find(id);
            if (estado == null)
            {
                return HttpNotFound();
            }

            return View(estado);
        }

        // POST: EstadosFinancieros/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EstadosFinancieros estado = db.EstadosFinancieros.Find(id);
            IndicadoresCalculados indicadores = db.IndicadoresCalculados.Find(id);
            if (estado != null)
            {
                db.EstadosFinancieros.Remove(estado);
                db.IndicadoresCalculados.Remove(indicadores);
                db.SaveChanges();
                TempData["Mensaje"] = "Estado Financiero eliminado exitosamente.";
            }

            return RedirectToAction("Index");
        }

        // POST: EstadosFinancieros/ActualizarEstado - Actualizar solo el checkbox Activo
        [HttpPost]
        public ActionResult ActualizarEstado(int id, bool activo)
        {
            try
            {
                var estado = db.EstadosFinancieros.Find(id);
                if (estado != null)
                {
                    estado.Activa = activo;
                    db.SaveChanges();
                    return Json(new { success = true, message = "Estado actualizado correctamente." });
                }

                return Json(new { success = false, message = "Estado no encontrado." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}