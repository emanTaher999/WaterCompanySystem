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
using Microsoft.Ajax.Utilities;

namespace WaterCompanySystem.Controllers
{
    public class Expense_TransactionController : Controller
    {
        private WaterComponySystemEntities db = new WaterComponySystemEntities();

        // GET: Expense_Transaction
        public ActionResult Index_Revenues()
        {
            var expense_Transaction = db.Expense_Transaction.Where(a=>a.transaction_type_id==13).Include(e => e.SuplierType).Include(e => e.TransactionType);
            return View(expense_Transaction.ToList());
        }

        // GET: Expense_Transaction/Details/5
        public ActionResult Details_Revenues(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Expense_Transaction expense_Transaction = db.Expense_Transaction.Find(id);
            if (expense_Transaction == null)
            {
                return HttpNotFound();
            }
            return View(expense_Transaction);
        }

        // GET: Expense_Transaction/Create
        public ActionResult Create_Revenues()
        {
            ViewBag.suplier_type_id = new SelectList(db.SuplierTypes.ToList(), "id", "suplier_type");
            ViewBag.transaction_type_id = new SelectList(db.TransactionTypes.Where(a => a.id == 13).ToList(), "id", "Transaction_type");
            return View();
        }

        // POST: Expense_Transaction/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create_Revenues([Bind(Include = "id,transaction_date,transaction_type_id,amount,quantity,suplier_type_id,note")] Expense_Transaction expense_Transaction)
        {
            try
            {

                int x = db.Expense_Transaction.Select(p => p.id).Cast<int?>().Max() ?? 0;
                int maxid = x + 1;
                expense_Transaction.id = maxid;
                expense_Transaction.transaction_type_id = 13;
                db.Expense_Transaction.Add(expense_Transaction);
                db.SaveChanges();
                TempData["AlertMessage"] = "success";
                return RedirectToAction("Index_Revenues");
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
                return View(expense_Transaction);
            }
            ViewBag.suplier_type_id = new SelectList(db.SuplierTypes.ToList(), "id", "suplier_type", expense_Transaction.suplier_type_id);
            ViewBag.transaction_type_id = new SelectList(db.TransactionTypes.Where(a => a.id == 13).ToList(), "id", "Transaction_type",expense_Transaction.transaction_type_id);
            return View(expense_Transaction);
        }

        // GET: Expense_Transaction/Edit/5
        public ActionResult Edit_Revenues(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Expense_Transaction expense_Transaction = db.Expense_Transaction.Find(id);
            //    int suplier_type_id = expense_Transaction.suplier_type_id.Value;
            if (expense_Transaction == null)
            {
                return HttpNotFound();
            }

            ViewBag.suplier_type_id = new SelectList(db.SuplierTypes, "id", "suplier_type", expense_Transaction.suplier_type_id);
            ViewBag.transaction_type_id = new SelectList(db.TransactionTypes.Where(a => a.id == 13), "id", "Transaction_type", expense_Transaction.transaction_type_id);
            return View(expense_Transaction);
        }

        // POST: Expense_Transaction/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit_Revenues")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit_Revenues([Bind(Include = "id,transaction_date,transaction_type_id,amount,quantity,suplier_type_id,note")] Expense_Transaction expense_Transaction,int id)
        {
           try
            { 

                db.Entry(expense_Transaction).State = EntityState.Modified;
                db.SaveChanges();
                TempData["AlertMessage"] = "edit";
                return RedirectToAction("Index_Revenues");
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
                return View(expense_Transaction);
            }

            ViewBag.suplier_type_id = new SelectList(db.SuplierTypes, "id", "suplier_type", expense_Transaction.suplier_type_id);
            ViewBag.transaction_type_id = new SelectList(db.TransactionTypes.Where(a => a.id == 13), "id", "Transaction_type", expense_Transaction.transaction_type_id);
            return View(expense_Transaction);
        }

        // GET: Expense_Transaction/Delete/5
        public ActionResult Delete_Revenues(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Expense_Transaction expense_Transaction = db.Expense_Transaction.Find(id);
            if (expense_Transaction == null)
            {
                return HttpNotFound();
            }

            ViewBag.suplier_type_id = new SelectList(db.SuplierTypes.ToList(), "id", "suplier_type", expense_Transaction.suplier_type_id);
            ViewBag.transaction_type_id = new SelectList(db.TransactionTypes.Where(a => a.id == 13), "id", "Transaction_type", expense_Transaction.transaction_type_id);

            return View(expense_Transaction);
        }

        // POST: Expense_Transaction/Delete/5
        [HttpPost, ActionName("Delete_Revenues")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed_Revenues(int id)
        {
            Expense_Transaction expense_Transaction = db.Expense_Transaction.Find(id);
            db.Expense_Transaction.Remove(expense_Transaction);
            db.SaveChanges();

            ViewBag.suplier_type_id = new SelectList(db.SuplierTypes.ToList(), "id", "suplier_type", expense_Transaction.suplier_type_id);
            ViewBag.transaction_type_id = new SelectList(db.TransactionTypes.Where(a => a.id == 13), "id", "Transaction_type", expense_Transaction.transaction_type_id);

            return RedirectToAction("Index_Revenues");
        }

        //
        ////////////////////////////////////////////////////////////////////////Dismissals//////////////////////////////////////////////////////
        //

        // GET: Expense_Transaction
        public ActionResult Index_Dismissals()
        {
                      //  ViewBag.transaction_type_id = new SelectList(db.TransactionTypes.Where(a => new[] {2,3,4,5,6,7,8,9,10 }.Contains(a.id)).ToList(), "id", "Transaction_type");

            var expense_Transaction = db.Expense_Transaction.Where(a => a.transaction_type_id == 2||a.transaction_type_id==3 || a.transaction_type_id == 4 ||
             a.transaction_type_id == 5 || a.transaction_type_id == 6 || a.transaction_type_id == 7 || a.transaction_type_id == 8 || a.transaction_type_id == 9
             || a.transaction_type_id == 10 || a.transaction_type_id == 11).Include(e => e.SuplierType).Include(e => e.TransactionType);
            return View(expense_Transaction.ToList());
        }

        // GET: Expense_Transaction/Details/5
        public ActionResult Details_Dismissals(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Expense_Transaction expense_Transaction = db.Expense_Transaction.Find(id);
            if (expense_Transaction == null)
            {
                return HttpNotFound();
            }
            return View(expense_Transaction);
        }

        // GET: Expense_Transaction/Create
        public ActionResult Create_Dismissals()
        {
            ViewBag.TransactionTypes = new SelectList(db.TransactionTypes.Where(a => new[] { 2, 3, 4, 5, 6, 7, 8, 9, 10,11}.Contains(a.id)).ToList(), "id", "Transaction_type");
         return View();
        }

        // POST: Expense_Transaction/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create_Dismissals(Expense_Transaction expense_Transaction)
        {
            try
            {

                int x = db.Expense_Transaction.Select(p => p.id).Cast<int?>().Max() ?? 0;
                int maxid = x + 1;
                expense_Transaction.id = maxid;
                db.Expense_Transaction.Add(expense_Transaction);
                db.SaveChanges();
                TempData["AlertMessage"] = "success";
                return RedirectToAction("Index_Dismissals");
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
                return View(expense_Transaction);
            }
            ViewBag.TransactionTypes = new SelectList(db.TransactionTypes.Where(a => new[] { 2, 3, 4, 5, 6, 7, 8, 9, 10,11 }.Contains(a.id)).ToList(), "id", "Transaction_type", expense_Transaction.transaction_type_id);
            return View(expense_Transaction);
        }

        // GET: Expense_Transaction/Edit/5
        public ActionResult Edit_Dismissals(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Expense_Transaction expense_Transaction = db.Expense_Transaction.Find(id);

            if (expense_Transaction == null)
            {
                return HttpNotFound();
            }
         
          //  ViewBag.suplier_type_id = new SelectList(db.SuplierTypes, "id", "nick", expense_Transaction.suplier_type_id);
            ViewBag.transaction_type_id = new SelectList(db.TransactionTypes.Where(a => new[] { 2, 3, 4, 5, 6, 7, 8, 9, 10,11 }.Contains(a.id)).ToList(), "id", "Transaction_type", expense_Transaction.transaction_type_id);
            return View(expense_Transaction);
        }

        // POST: Expense_Transaction/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit_Dismissals([Bind(Include = "id,transaction_date,transaction_type_id,amount,quantity,suplier_type_id,note")] Expense_Transaction expense_Transaction)
        {
           try
            {

                db.Entry(expense_Transaction).State = EntityState.Modified;
                db.SaveChanges();
                TempData["AlertMessage"] = "edit";
                return RedirectToAction("Index_Dismissals");
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
                return View(expense_Transaction);
            }
            //   ViewBag.suplier_type_id = new SelectList(db.SuplierTypes, "id", "nick", expense_Transaction.suplier_type_id);
            ViewBag.transaction_type_id = new SelectList(db.TransactionTypes.Where(a => new[] { 2, 3, 4, 5, 6, 7, 8, 9, 10,11 }.Contains(a.id)).ToList(), "id", "Transaction_type", expense_Transaction.transaction_type_id);
            return View(expense_Transaction);
        }

        // GET: Expense_Transaction/Delete/5
        public ActionResult Delete_Dismissals(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Expense_Transaction expense_Transaction = db.Expense_Transaction.Find(id);
            if (expense_Transaction == null)
            {
                return HttpNotFound();
            }
          //  ViewBag.suplier_type_id = new SelectList(db.SuplierTypes, "id", "nick", expense_Transaction.suplier_type_id);
            ViewBag.transaction_type_id = new SelectList(db.TransactionTypes.Where(a => new[] { 2, 3, 4, 5, 6, 7, 8, 9, 10,11 }.Contains(a.id)).ToList(), "id", "Transaction_type", expense_Transaction.transaction_type_id);

            return View(expense_Transaction);
        }

        // POST: Expense_Transaction/Delete/5
        [HttpPost, ActionName("Delete_Dismissals")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed_Dismissals(int id)
        {
            Expense_Transaction expense_Transaction = db.Expense_Transaction.Find(id);
            db.Expense_Transaction.Remove(expense_Transaction);
            db.SaveChanges();

            //ViewBag.suplier_type_id = new SelectList(db.SuplierTypes, "id", "nick", expense_Transaction.suplier_type_id);
            ViewBag.transaction_type_id = new SelectList(db.TransactionTypes.Where(a => new[] { 2, 3, 4, 5, 6, 7, 8, 9, 10,11 }.Contains(a.id)).ToList(), "id", "Transaction_type", expense_Transaction.transaction_type_id);

            return RedirectToAction("Index_Dismissals");
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
      
        public JsonResult GetPrice(int suplier_type_id)
        {
            // Assume you have a database context or repository to fetch data
            using (var context = new WaterComponySystemEntities())
            {
                var price = context.SuplierTypePrices
                                   .Where(s => s.id == suplier_type_id)
                                   .Select(s => s.price)
                                   .FirstOrDefault();

                return Json(new { price = price }, JsonRequestBehavior.AllowGet);
            }
        }

       
        public JsonResult GetTOTAL(string price, string quantity)
        {
            decimal _amount = Convert.ToDecimal(price);
            int _quantity = int.Parse(quantity);

            var TotalAmount = (_amount * _quantity);
            if (TotalAmount != null)
            {
                var data = TotalAmount;
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(false, JsonRequestBehavior.AllowGet);

        }
    }
}
