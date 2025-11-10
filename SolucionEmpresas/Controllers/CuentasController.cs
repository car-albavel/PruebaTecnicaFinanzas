using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using SolucionEmpresas.Models;


namespace SolucionEmpresas.Controllers
{
    public class CuentasController : Controller
    {
        private dbFinanzasEmpresaEntities db = new  dbFinanzasEmpresaEntities();

        // GET: Cuentas
        public ActionResult Index(int? empresaId)
        {
            var cuentas = db.Cuentas.Where(c => !empresaId.HasValue || c.EmpresaId == empresaId).ToList();
            ViewBag.EmpresaId = empresaId;
            return View(cuentas);
        }

        // GET: Cuentas/Create
        public ActionResult Create(int? empresaId)
        {
            var empresas = db.Empresas.Where(e => e.Estado).ToList();
            ViewBag.Empresas = new SelectList(empresas, "Id", "Nombre", empresaId);
            return View();
        }

        // POST: Cuentas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EmpresaId,Numero,Nombre")] Cuentas cuenta)
        {
            if (ModelState.IsValid)
            {
                cuenta.FechaCreacion = DateTime.Now;
                cuenta.Activa = true;
                db.Cuentas.Add(cuenta);
                db.SaveChanges();
                return RedirectToAction("Index", new { empresaId = cuenta.EmpresaId });
            }

            var empresas = db.Empresas.Where(e => e.Estado).ToList();
            ViewBag.Empresas = new SelectList(empresas, "Id", "Nombre", cuenta.EmpresaId);
            return View(cuenta);
        }

        // GET: Cuentas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Cuentas cuenta = db.Cuentas.Find(id);
            if (cuenta == null)
            {
                return HttpNotFound();
            }

            var empresas = db.Empresas.Where(e => e.Estado).ToList();
            ViewBag.Empresas = new SelectList(empresas, "Id", "Nombre", cuenta.EmpresaId);
            return View(cuenta);
        }

        // POST: Cuentas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,EmpresaId,Numero,Nombre,Activa")] Cuentas cuenta)
        {
            if (ModelState.IsValid)
            {
                var cuentaExistente = db.Cuentas.Find(cuenta.Id);
                if (cuentaExistente != null)
                {
                    cuentaExistente.Numero = cuenta.Numero;
                    cuentaExistente.Nombre = cuenta.Nombre;
                    cuentaExistente.Activa = cuenta.Activa;
                    db.SaveChanges();
                }

                return RedirectToAction("Index", new { empresaId = cuenta.EmpresaId });
            }

            var empresas = db.Empresas.Where(e => e.Estado).ToList();
            ViewBag.Empresas = new SelectList(empresas, "Id", "Nombre", cuenta.EmpresaId);
            return View(cuenta);
        }

        // GET: Cuentas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Cuentas cuenta = db.Cuentas.Find(id);
            if (cuenta == null)
            {
                return HttpNotFound();
            }

            return View(cuenta);
        }

        // POST: Cuentas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cuentas cuenta = db.Cuentas.Find(id);
            if (cuenta != null)
            {
                cuenta.Activa = false;
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