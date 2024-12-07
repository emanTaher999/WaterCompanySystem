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
    public class SuplierTypePricesController : Controller
    {
        private WaterComponySystemEntities db = new WaterComponySystemEntities();

        // GET: SuplierTypePrices
        public ActionResult Index()
        {
            var suplierTypePrices = db.SuplierTypePrices.Include(s => s.SuplierType);
            return View(suplierTypePrices.ToList());
        }

        // GET: SuplierTypePrices/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SuplierTypePrice suplierTypePrice = db.SuplierTypePrices.Find(id);
            if (suplierTypePrice == null)
            {
                return HttpNotFound();
            }
            return View(suplierTypePrice);
        }

        // GET: SuplierTypePrices/Create
        public ActionResult Create()
        {
            ViewBag.suplier_Type_id = new SelectList(db.SuplierTypes, "id", "suplier_type");
            return View();
        }

        // POST: SuplierTypePrices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,suplier_Type_id,nick,price")] SuplierTypePrice suplierTypePrice)
        {
            if (ModelState.IsValid)
            {
                db.SuplierTypePrices.Add(suplierTypePrice);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.suplier_Type_id = new SelectList(db.SuplierTypes, "id", "suplier_type", suplierTypePrice.suplier_Type_id);
            return View(suplierTypePrice);
        }

        // GET: SuplierTypePrices/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SuplierTypePrice suplierTypePrice = db.SuplierTypePrices.Find(id);
            if (suplierTypePrice == null)
            {
                return HttpNotFound();
            }
            ViewBag.suplier_Type_id = new SelectList(db.SuplierTypes, "id", "suplier_type", suplierTypePrice.suplier_Type_id);
            return View(suplierTypePrice);
        }

        // POST: SuplierTypePrices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,suplier_Type_id,nick,price")] SuplierTypePrice suplierTypePrice)
        {
            if (ModelState.IsValid)
            {
                db.Entry(suplierTypePrice).State = EntityState.Modified;
                db.SaveChanges();
                TempData["AlertMessage"] = "edit";
                return RedirectToAction("Index");
            }
            ViewBag.suplier_Type_id = new SelectList(db.SuplierTypes, "id", "suplier_type", suplierTypePrice.suplier_Type_id);
            return View(suplierTypePrice);
        }

        // GET: SuplierTypePrices/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SuplierTypePrice suplierTypePrice = db.SuplierTypePrices.Find(id);
            if (suplierTypePrice == null)
            {
                return HttpNotFound();
            }
          //  ViewBag.suplier_Type_id = new SelectList(db.SuplierTypes, "id", "suplier_type", suplierTypePrice.suplier_Type_id);

            return View(suplierTypePrice);
        }

        // POST: SuplierTypePrices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SuplierTypePrice suplierTypePrice = db.SuplierTypePrices.Find(id);
            db.SuplierTypePrices.Remove(suplierTypePrice);
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
