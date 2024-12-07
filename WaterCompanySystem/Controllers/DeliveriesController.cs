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
    public class DeliveriesController : Controller
    {
        private WaterComponySystemEntities db = new WaterComponySystemEntities();

        // GET: Deliveries
        public ActionResult Index()
        {
            var deliveries = db.Deliveries.Include(d => d.Custmor).Include(d => d.DeliveryStatu).Include(d => d.Employee);
            return View(deliveries.ToList());
        }

        // GET: Deliveries/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Delivery delivery = db.Deliveries.Find(id);
            if (delivery == null)
            {
                return HttpNotFound();
            }
            return View(delivery);
        }

        // GET: Deliveries/Create
        public ActionResult Create()
        {
            ViewBag.custmor_id = new SelectList(db.Custmors, "id", "cusmor_name");
            ViewBag.delivery_status_id = new SelectList(db.DeliveryStatus, "id", "del_status");
            ViewBag.employee_id = new SelectList(db.Employees, "id", "emp_name");
            return View();
        }

        // POST: Deliveries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,custmor_id,delivery_date,num_bottleS_delivered,delivery_status_id,creat_at,employee_id")] Delivery delivery)
        {
            if (ModelState.IsValid)
            {
                db.Deliveries.Add(delivery);
                db.SaveChanges();
                TempData["AlertMessage"] = "edit";
                return RedirectToAction("Index");
            }

            ViewBag.custmor_id = new SelectList(db.Custmors, "id", "cusmor_name", delivery.custmor_id);
            ViewBag.delivery_status_id = new SelectList(db.DeliveryStatus, "id", "del_status", delivery.delivery_status_id);
            ViewBag.employee_id = new SelectList(db.Employees, "id", "emp_name", delivery.employee_id);
            return View(delivery);
        }

        // GET: Deliveries/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Delivery delivery = db.Deliveries.Find(id);
            if (delivery == null)
            {
                return HttpNotFound();
            }
            ViewBag.custmor_id = new SelectList(db.Custmors, "id", "cusmor_name", delivery.custmor_id);
            ViewBag.delivery_status_id = new SelectList(db.DeliveryStatus, "id", "del_status", delivery.delivery_status_id);
            ViewBag.employee_id = new SelectList(db.Employees, "id", "emp_name", delivery.employee_id);
            return View(delivery);
        }

        // POST: Deliveries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,custmor_id,delivery_date,num_bottleS_delivered,delivery_status_id,creat_at,employee_id")] Delivery delivery)
        {
            if (ModelState.IsValid)
            {
                db.Entry(delivery).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.custmor_id = new SelectList(db.Custmors, "id", "cusmor_name", delivery.custmor_id);
            ViewBag.delivery_status_id = new SelectList(db.DeliveryStatus, "id", "del_status", delivery.delivery_status_id);
            ViewBag.employee_id = new SelectList(db.Employees, "id", "emp_name", delivery.employee_id);
            return View(delivery);
        }

        // GET: Deliveries/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Delivery delivery = db.Deliveries.Find(id);
            if (delivery == null)
            {
                return HttpNotFound();
            }
            return View(delivery);
        }

        // POST: Deliveries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Delivery delivery = db.Deliveries.Find(id);
            db.Deliveries.Remove(delivery);
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
