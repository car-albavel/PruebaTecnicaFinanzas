using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SolucionEmpresas.Models;

namespace SolucionEmpresas.Controllers
{
    public class ConfigurarIndicadoresController : Controller
    {
        private dbFinanzasEmpresaEntities db = new dbFinanzasEmpresaEntities();

                // GET: Indicadores
        public ActionResult Index()
        {
            var indicadores = db.ConfigurarIndicadores
                                .OrderBy(i => i.Indicador)
                                .ToList();

            return View(indicadores);
        }

        // GET: Indicadores/Edit?indicador=Indicador1
        public ActionResult Edit(string indicador)
        {
            if (string.IsNullOrEmpty(indicador))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var entidad = db.ConfigurarIndicadores
                            .FirstOrDefault(i => i.Indicador == indicador);

            if (entidad == null)
            {
                return HttpNotFound();
            }

            return View(entidad);
        }

        // POST: Indicadores/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Indicador,Activo,NombreIndicador,Formula,Descripcion")] ConfigurarIndicadores modelo)
        {
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            //Recalculamos el indicador si se activa, si se desactiva se dejan los valores en cero
            if(modelo.Activo && modelo.Formula.Length > 10)
            {
                string instrccion = " t2." + modelo.Indicador + " = " + modelo.Formula;
                db.ActualizarIndicador1(instrccion);
            }
            else
            {
                string instrccion = " t2." + modelo.Indicador + " = 0";
                db.ActualizarIndicador1(instrccion);
            }

            
            var entidad = db.ConfigurarIndicadores
                            .FirstOrDefault(i => i.Indicador == modelo.Indicador);

            if (entidad == null)
            {
                return HttpNotFound();
            }

            // Actualizamos solo los campos editables
            entidad.Activo = modelo.Activo;
            entidad.NombreIndicador = modelo.NombreIndicador;
            entidad.Formula = modelo.Formula;
            entidad.Descripcion = modelo.Descripcion;

            db.Entry(entidad).State = EntityState.Modified;
            db.SaveChanges();

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
