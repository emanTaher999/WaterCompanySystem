using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WaterCompanySystem.Models;

namespace WaterCompanySystem.Controllers
{
    public class BottleTypesController : Controller
    {
        private WaterComponySystemEntities db = new WaterComponySystemEntities();

        // GET: BottleTypes
        public ActionResult Index()
        {
            return View(db.BottleTypes.ToList());
        }

        // GET: BottleTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BottleType bottleType = db.BottleTypes.Find(id);
            if (bottleType == null)
            {
                return HttpNotFound();
            }
            return View(bottleType);
        }

        // GET: BottleTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BottleTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,bottle_type")] BottleType bottleType)
        {
            try
            {

                int x = db.BottleTypes.Select(p => p.id).Cast<int?>().Max() ?? 0;
                int maxid = x + 1;
                bottleType.id = maxid;
                db.BottleTypes.Add(bottleType);
                db.SaveChanges();
                TempData["AlertMessage"] = "success";
                return RedirectToAction("Index");

            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                return View(bottleType);
            }
            
            return View(bottleType);

        }

        // GET: BottleTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BottleType bottleType = db.BottleTypes.Find(id);
            if (bottleType == null)
            {
                return HttpNotFound();
            }
            return View(bottleType);
        }

        // POST: BottleTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,bottle_type")] BottleType bottleType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bottleType).State = EntityState.Modified;
                db.SaveChanges();
                TempData["AlertMessage"] = "edit";
                return RedirectToAction("Index");
            }
            return View(bottleType);
        }

        // GET: BottleTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BottleType bottleType = db.BottleTypes.Find(id);
            if (bottleType == null)
            {
                return HttpNotFound();
            }
            return View(bottleType);
        }

        // POST: BottleTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BottleType bottleType = db.BottleTypes.Find(id);
            db.BottleTypes.Remove(bottleType);
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
