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
    public class SuplierNamesController : Controller
    {
        private WaterComponySystemEntities db = new WaterComponySystemEntities();

        // GET: SuplierNames
        public ActionResult Index()
        {
            return View(db.SuplierNames.ToList());
        }

        // GET: SuplierNames/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SuplierName suplierName = db.SuplierNames.Find(id);
            if (suplierName == null)
            {
                return HttpNotFound();
            }
            return View(suplierName);
        }

        // GET: SuplierNames/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SuplierNames/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,suplier_name,location_cus,contact_number,email,fax")] SuplierName suplierName)
        {
            if (ModelState.IsValid)
            {
                db.SuplierNames.Add(suplierName);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(suplierName);
        }

        // GET: SuplierNames/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SuplierName suplierName = db.SuplierNames.Find(id);
            if (suplierName == null)
            {
                return HttpNotFound();
            }
            return View(suplierName);
        }

        // POST: SuplierNames/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,suplier_name,location_cus,contact_number,email,fax")] SuplierName suplierName)
        {
            if (ModelState.IsValid)
            {
                db.Entry(suplierName).State = EntityState.Modified;
                db.SaveChanges();

                TempData["AlertMessage"] = "edit";
                return RedirectToAction("Index");
            }
            return View(suplierName);
        }

        // GET: SuplierNames/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SuplierName suplierName = db.SuplierNames.Find(id);
            if (suplierName == null)
            {
                return HttpNotFound();
            }
            return View(suplierName);
        }

        // POST: SuplierNames/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SuplierName suplierName = db.SuplierNames.Find(id);
            db.SuplierNames.Remove(suplierName);
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
