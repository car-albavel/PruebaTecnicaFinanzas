using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using SolucionEmpresas.Models;

namespace TuAplicacion.Controllers
{
    public class IndicadoresController : Controller
    {
        private dbFinanzasEmpresaEntities db = new dbFinanzasEmpresaEntities();

        // GET: Indicadores
        public ActionResult Index(int? empresaId, int? anio, int? mes)
        {
            var indicadores = db.Indicadores.AsQueryable();

            if (empresaId.HasValue)
            {
                indicadores = indicadores.Where(i => i.EmpresaId == empresaId);
            }

            if (anio.HasValue)
            {
                indicadores = indicadores.Where(i => i.Periodo.Year == anio);
            }

            if (mes.HasValue)
            {
                indicadores = indicadores.Where(i => i.Periodo.Month == mes);
            }

            ViewBag.Empresas = new SelectList(db.Empresas.Where(e => e.Estado), "EmpresaId", "NombreEmpresa", empresaId);
            ViewBag.EmpresaId = empresaId;
            ViewBag.Anio = anio;
            ViewBag.Mes = mes;

            return View(indicadores.ToList());
        }

        // GET: Indicadores/Create
        public ActionResult Create(int? empresaId)
        {
            var empresas = db.Empresas.Where(e => e.Estado).ToList();
            ViewBag.Empresas = new SelectList(empresas, "Id", "Nombre", empresaId);
            return View();
        }

        // POST: Indicadores/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EmpresaId,Nombre,Descripcion,Formula,Valor,Periodo")] Indicadores indicador)
        {
            if (ModelState.IsValid)
            {
                indicador.Activo = true;
                db.Indicadores.Add(indicador);
                db.SaveChanges();
                return RedirectToAction("Index", new { empresaId = indicador.EmpresaId });
            }

            var empresas = db.Empresas.Where(e => e.Estado).ToList();
            ViewBag.Empresas = new SelectList(empresas, "Id", "Nombre", indicador.EmpresaId);
            return View(indicador);
        }

        // GET: Indicadores/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Indicadores indicador = db.Indicadores.Find(id);
            if (indicador == null)
            {
                return HttpNotFound();
            }

            var empresas = db.Empresas.Where(e => e.Estado).ToList();
            ViewBag.Empresas = new SelectList(empresas, "Id", "Nombre", indicador.EmpresaId);
            return View(indicador);
        }

        // POST: Indicadores/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,EmpresaId,Nombre,Descripcion,Formula,Valor,Periodo,Activo")] Indicadores indicador)
        {
            if (ModelState.IsValid)
            {
                var indicadorExistente = db.Indicadores.Find(indicador.Id);
                if (indicadorExistente != null)
                {
                    indicadorExistente.Nombre = indicador.Nombre;
                    indicadorExistente.Descripcion = indicador.Descripcion;
                    indicadorExistente.Formula = indicador.Formula;
                    indicadorExistente.Valor = indicador.Valor;
                    indicadorExistente.Periodo = indicador.Periodo;
                    indicadorExistente.Activo = indicador.Activo;
                    db.SaveChanges();
                }

                return RedirectToAction("Index", new { empresaId = indicador.EmpresaId });
            }

            var empresas = db.Empresas.Where(e => e.Estado).ToList();
            ViewBag.Empresas = new SelectList(empresas, "Id", "Nombre", indicador.EmpresaId);
            return View(indicador);
        }

        // GET: Indicadores/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Indicadores indicador = db.Indicadores.Find(id);
            if (indicador == null)
            {
                return HttpNotFound();
            }

            return View(indicador);
        }

        // POST: Indicadores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Indicadores indicador = db.Indicadores.Find(id);
            if (indicador != null)
            {
                indicador.Activo = false;
                db.SaveChanges();
            }

            return RedirectToAction("Index");
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