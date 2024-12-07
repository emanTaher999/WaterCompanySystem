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
    public class UserTablesController : Controller
    {
        private     WaterComponySystemEntities db = new WaterComponySystemEntities();

        // GET: UserTables
        public ActionResult Index()
        {
            var userTables = db.UserTables.Include(u => u.RoleTable);
            return View(userTables.ToList());
        }

        // GET: UserTables/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserTable userTable = db.UserTables.Find(id);
            if (userTable == null)
            {
                return HttpNotFound();
            }
            return View(userTable);
        }

        // GET: UserTables/Create
        public ActionResult Create()
        {
            ViewBag.role_id = new SelectList(db.RoleTables, "id", "role");
            return View();
        }

        // POST: UserTables/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,username,password,role_id")] UserTable userTable)
        {
            if (ModelState.IsValid)
            {
                db.UserTables.Add(userTable);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.role_id = new SelectList(db.RoleTables, "id", "role", userTable.role_id);
            return View(userTable);
        }

        // GET: UserTables/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserTable userTable = db.UserTables.Find(id);
            if (userTable == null)
            {
                return HttpNotFound();
            }
            ViewBag.role_id = new SelectList(db.RoleTables, "id", "role", userTable.role_id);
            return View(userTable);
        }

        // POST: UserTables/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,username,password,role_id")] UserTable userTable)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userTable).State = EntityState.Modified;
                db.SaveChanges();
                TempData["AlertMessage"] = "edit";
                return RedirectToAction("Index");
            }
            ViewBag.role_id = new SelectList(db.RoleTables, "id", "role", userTable.role_id);
            return View(userTable);
        }

        // GET: UserTables/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserTable userTable = db.UserTables.Find(id);
            if (userTable == null)
            {
                return HttpNotFound();
            }
            return View(userTable);
        }

        // POST: UserTables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserTable userTable = db.UserTables.Find(id);
            db.UserTables.Remove(userTable);
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
