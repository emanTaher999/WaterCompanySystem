using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WaterCompanySystem.Models;

namespace WaterCompanySystem.Controllers
{
    public class SuplierTypesController : Controller
    {
        private WaterComponySystemEntities db = new WaterComponySystemEntities();

        // GET: SuplierTypes
        public ActionResult Index()
        {
            var suplierTypes = db.SuplierTypes.Include(s => s.SuplierName);
            return View(suplierTypes.ToList());
        }

        // GET: SuplierTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SuplierType suplierType = db.SuplierTypes.Find(id);
            if (suplierType == null)
            {
                return HttpNotFound();
            }
            return View(suplierType);
        }

        // GET: SuplierTypes/Create
        public ActionResult Create()
        {
            ViewBag.suplier_id = new SelectList(db.SuplierNames, "id", "suplier_name");
            return View();
        }

        // POST: SuplierTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,suplier_id,suplier_type")] SuplierType suplierType)
        {
            if (ModelState.IsValid)
            {
                db.SuplierTypes.Add(suplierType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.suplier_id = new SelectList(db.SuplierNames, "id", "suplier_name", suplierType.suplier_id);
            return View(suplierType);
        }

        // GET: SuplierTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SuplierType suplierType = db.SuplierTypes.Find(id);
            if (suplierType == null)
            {
                return HttpNotFound();
            }
            ViewBag.suplier_id = new SelectList(db.SuplierNames, "id", "suplier_name", suplierType.suplier_id);
            return View(suplierType);
        }

        // POST: SuplierTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,suplier_id,suplier_type")] SuplierType suplierType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(suplierType).State = EntityState.Modified;
                db.SaveChanges();
                TempData["AlertMessage"] = "edit";
                return RedirectToAction("Index");
            }
            ViewBag.suplier_id = new SelectList(db.SuplierNames, "id", "suplier_name", suplierType.suplier_id);
            return View(suplierType);
        }

        // GET: SuplierTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SuplierType suplierType = db.SuplierTypes.Find(id);
            if (suplierType == null)
            {
                return HttpNotFound();
            }
            return View(suplierType);
        }

        // POST: SuplierTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SuplierType suplierType = db.SuplierTypes.Find(id);
            db.SuplierTypes.Remove(suplierType);
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
