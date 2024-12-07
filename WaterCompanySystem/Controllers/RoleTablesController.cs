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
    public class RoleTablesController : Controller
    {
        private WaterComponySystemEntities db = new WaterComponySystemEntities();

        // GET: RoleTables
        public ActionResult Index()
        {
            return View(db.RoleTables.ToList());
        }

        // GET: RoleTables/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoleTable roleTable = db.RoleTables.Find(id);
            if (roleTable == null)
            {
                return HttpNotFound();
            }
            return View(roleTable);
        }

        // GET: RoleTables/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RoleTables/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,role")] RoleTable roleTable)
        {
            if (ModelState.IsValid)
            {
                db.RoleTables.Add(roleTable);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(roleTable);
        }

        // GET: RoleTables/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoleTable roleTable = db.RoleTables.Find(id);
            if (roleTable == null)
            {
                return HttpNotFound();
            }
            return View(roleTable);
        }

        // POST: RoleTables/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,role")] RoleTable roleTable)
        {
            if (ModelState.IsValid)
            {
                db.Entry(roleTable).State = EntityState.Modified;
                db.SaveChanges();
                TempData["AlertMessage"] = "edit";

                return RedirectToAction("Index");
            }
            return View(roleTable);
        }

        // GET: RoleTables/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoleTable roleTable = db.RoleTables.Find(id);
            if (roleTable == null)
            {
                return HttpNotFound();
            }
            return View(roleTable);
        }

        // POST: RoleTables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RoleTable roleTable = db.RoleTables.Find(id);
            db.RoleTables.Remove(roleTable);
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
