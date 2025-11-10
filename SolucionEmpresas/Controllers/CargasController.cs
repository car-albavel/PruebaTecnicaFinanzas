using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SolucionEmpresas.Models;

namespace SolucionEmpresas.Controllers
{
    public class CargasController : Controller
    {
        private dbFinanzasEmpresaEntities db = new dbFinanzasEmpresaEntities();

        // GET: Cargas
        public ActionResult Index(int? empresaId)
        {
            var cargas = db.CargasArchivos.AsQueryable();

            if (empresaId.HasValue)
            {
                cargas = cargas.Where(c => c.EmpresaId == empresaId);
            }

            var empresas = db.Empresas.Where(e => e.Estado).ToList();
            ViewBag.Empresas = new SelectList(empresas, "Id", "Nombre", empresaId);
            ViewBag.EmpresaId = empresaId;

            return View(cargas.ToList());
        }

        // GET: Cargas/Create
        public ActionResult Create(int? empresaId)
        {
            var empresas = db.Empresas.Where(e => e.Estado).ToList();
            ViewBag.Empresas = new SelectList(empresas, "Id", "Nombre", empresaId);
            return View();
        }

        // POST: Cargas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int empresaId, HttpPostedFileBase archivo)
        {
            if (archivo != null && archivo.ContentLength > 0)
            {
                try
                {
                    string extension = Path.GetExtension(archivo.FileName).ToLower();

                    if (extension != ".xlsx" && extension != ".xls" && extension != ".pdf")
                    {
                        ModelState.AddModelError("", "Solo se permiten archivos Excel o PDF");
                        var empresas = db.Empresas.Where(e => e.Estado).ToList();
                        ViewBag.Empresas = new SelectList(empresas, "Id", "Nombre", empresaId);
                        return View();
                    }

                    string rutaArchivos = Server.MapPath("~/Uploads/Contables/");
                    if (!Directory.Exists(rutaArchivos))
                    {
                        Directory.CreateDirectory(rutaArchivos);
                    }

                    string nombreArchivo = Guid.NewGuid().ToString() + extension;
                    string rutaCompleta = Path.Combine(rutaArchivos, nombreArchivo);

                    archivo.SaveAs(rutaCompleta);

                    var carga = new CargasArchivos
                    {
                        EmpresaId = empresaId,
                        NombreArchivo = archivo.FileName,
                        RutaArchivo = rutaCompleta,
                        FechaCarga = DateTime.Now,
                        TipoArchivo = extension.Replace(".", "").ToUpper(),
                        Procesado = false
                    };

                    CargasArchivos cargasArchivos = db.CargasArchivos.Add(carga);
                    db.SaveChanges();

                    ViewBag.Mensaje = "Archivo cargado exitosamente.";
                    return RedirectToAction("Index", new { empresaId = empresaId });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error al cargar el archivo: " + ex.Message);
                }
            }
            else
            {
                ModelState.AddModelError("", "Por favor seleccione un archivo");
            }

            var empresasLista = db.Empresas.Where(e => e.Estado).ToList();
            ViewBag.Empresas = new SelectList(empresasLista, "Id", "Nombre", empresaId);
            return View();
        }

        // GET: Cargas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CargasArchivos carga = db.CargasArchivos.Find(id);
            if (carga == null)
            {
                return HttpNotFound();
            }

            return View(carga);
        }

        // POST: Cargas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CargasArchivos carga = db.CargasArchivos.Find(id);
            if (carga != null)
            {
                if (System.IO.File.Exists(carga.RutaArchivo))
                {
                    System.IO.File.Delete(carga.RutaArchivo);
                }

                db.CargasArchivos.Remove(carga);
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