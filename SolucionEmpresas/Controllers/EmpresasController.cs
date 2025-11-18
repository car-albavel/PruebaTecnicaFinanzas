using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using SolucionEmpresas.Models;

namespace SolucionEmpresas.Controllers
{
    public class EmpresasController : Controller
    {
        dbFinanzasEmpresaEntities db = new dbFinanzasEmpresaEntities();

        // GET: Empresas
        public ActionResult Index()
        {
            var empresas = db.Empresas.ToList();
            return View(empresas);
        }

        // GET: Empresas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Empresas empresa = db.Empresas.Find(id);
            if (empresa == null)
            {
                return HttpNotFound();
            }

            return View(empresa);
        }

        // GET: Empresas/Create
        public ActionResult Create()
        {
            var empresa = new Empresas();
            return View(empresa);
        }

        // POST: Empresas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NombreEmpresa,NIT,Direccion,Telefono,Activa")] Empresas empresa)
        {
            if (ModelState.IsValid)
            {
                empresa.FechaCreacion = DateTime.Now;
                empresa.Estado = true;
                db.Empresas.Add(empresa);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(empresa);
        }

        // GET: Empresas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Empresas empresa = db.Empresas.Find(id);
            if (empresa == null)
            {
                return HttpNotFound();
            }

            return View(empresa);
        }

        // POST: Empresas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre,NIT,Direccion,Telefono,Activa")] Empresas empresa)
        {
            if (ModelState.IsValid)
            {
                var empresaExistente = db.Empresas.Find(empresa.EmpresaId);
                if (empresaExistente != null)
                {
                    empresaExistente.NombreEmpresa = empresa.NombreEmpresa;
                    empresaExistente.NIT = empresa.NIT;
                    empresaExistente.Direccion = empresa.Direccion;
                    empresaExistente.Telefono = empresa.Telefono;
                    empresaExistente.Estado = empresa.Estado;

                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }

            return View(empresa);
        }

        // GET: Empresas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Empresas empresa = db.Empresas.Find(id);
            if (empresa == null)
            {
                return HttpNotFound();
            }

            var cuentasAsociadas = db.EstadosFinancieros.FirstOrDefault(c => c.EmpresaId == id);
            if(cuentasAsociadas != null)
            {
                TempData["Mensaje"] = "Esta Empresa tiene cuentas asociadas";
            }

            return View(empresa);
        }

        // POST: Empresas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var cuentasAsociadas = db.EstadosFinancieros.FirstOrDefault(c => c.EmpresaId == id);
            if (cuentasAsociadas != null)
            {
                TempData["Mensaje2"] = "Esta Empresa tiene cuentas asociadas";
                Empresas empresa2 = db.Empresas.Find(id);
                return View(empresa2);
            }


            Empresas empresa = db.Empresas.Find(id);
            if (empresa != null)
            {
                db.Empresas.Remove(empresa);
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