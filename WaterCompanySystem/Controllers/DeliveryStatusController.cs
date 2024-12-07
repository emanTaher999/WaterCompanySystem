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
    public class DeliveryStatusController : Controller
    {
        private WaterComponySystemEntities db = new WaterComponySystemEntities();

        // GET: DeliveryStatus
        public ActionResult Index()
        {
            return View(db.DeliveryStatus.ToList());
        }

        // GET: DeliveryStatus/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeliveryStatu deliveryStatu = db.DeliveryStatus.Find(id);
            if (deliveryStatu == null)
            {
                return HttpNotFound();
            }
            return View(deliveryStatu);
        }

        // GET: DeliveryStatus/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DeliveryStatus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,del_status")] DeliveryStatu deliveryStatu)
        {
            if (ModelState.IsValid)
            {
                db.DeliveryStatus.Add(deliveryStatu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(deliveryStatu);
        }

        // GET: DeliveryStatus/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeliveryStatu deliveryStatu = db.DeliveryStatus.Find(id);
            if (deliveryStatu == null)
            {
                return HttpNotFound();
            }
            return View(deliveryStatu);
        }

        // POST: DeliveryStatus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,del_status")] DeliveryStatu deliveryStatu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(deliveryStatu).State = EntityState.Modified;
                db.SaveChanges();
                TempData["AlertMessage"] = "edit";
                return RedirectToAction("Index");
            }
            return View(deliveryStatu);
        }

        // GET: DeliveryStatus/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeliveryStatu deliveryStatu = db.DeliveryStatus.Find(id);
            if (deliveryStatu == null)
            {
                return HttpNotFound();
            }
            return View(deliveryStatu);
        }

        // POST: DeliveryStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DeliveryStatu deliveryStatu = db.DeliveryStatus.Find(id);
            db.DeliveryStatus.Remove(deliveryStatu);
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
