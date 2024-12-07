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
    public class Income_TransactionController : Controller
    {
        private WaterComponySystemEntities db = new WaterComponySystemEntities();

        // GET: Income_Transaction
        public ActionResult Index()
        {
            //var income_Transaction = db.Income_Transaction.Include(i => i.Payment_Type).Include(i => i.SuplierType).Include(i => i.TransactionType);
            var income_Transaction = db.Income_Transaction.Where(a => a.transaction_type_id == 1 || a.transaction_type_id == 12).Include(i => i.Payment_Type).ToList();//.Include(i => i.SuplierType).Include(i => i.TransactionType).Include(i=>i.Custmor);

            return View(income_Transaction);
        }
       
        // GET: Income_Transaction/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Income_Transaction income_Transaction = db.Income_Transaction.Find(id);
            if (income_Transaction == null)
            {
                return HttpNotFound();
            }
            return View(income_Transaction);
        }

        // GET: Income_Transaction/Create
        public ActionResult Create()    
        {
            ViewBag.custmor_id = new SelectList(db.Custmors, "id", "cusmor_name");
            ViewBag.payment_type_id = new SelectList(db.Payment_Type, "id", "payment");
            ViewBag.suplier_type_id_nick = new SelectList(db.SuplierTypePrices, "id", "nick");
          // ViewBag.suplier_type_id = new SelectList(db.SuplierTypePrices, "id", "nick");
            ViewBag.transaction_type_id = new SelectList(db.TransactionTypes.Where(a => new[] { 1, 12 }.Contains(a.id)).ToList(), "id", "Transaction_type");
            return View();
        }

        // POST: Income_Transaction/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,transaction_date,transaction_type_id,amount,quantity,suplier_type_id,payment_type_id,custmor_id,note")] Income_Transaction income_Transaction)
        {
            try
            {
                int x = db.Income_Transaction.Select(p => p.id).Cast<int?>().Max() ?? 0;
                int maxid = x + 1;
                income_Transaction.id = maxid;
               // income_Transaction.transaction_date = System.DateTime.Now;
              
                db.Income_Transaction.Add(income_Transaction);
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

            }
            ViewBag.custmor_id = new SelectList(db.Custmors, "id", "cusmor_name", income_Transaction.custmor_id);
            ViewBag.payment_type_id = new SelectList(db.Payment_Type, "id", "payment", income_Transaction.payment_type_id);
           ViewBag.suplier_type_id_nick = new SelectList(db.SuplierTypePrices, "id", "nick",income_Transaction.suplier_type_id);
          //  ViewBag.suplier_type_id = new SelectList(db.SuplierTypes, "id", "suplier_type", income_Transaction.suplier_type_id);
            ViewBag.transaction_type_id = new SelectList(db.TransactionTypes.Where(a => new[] { 1, 12 }.Contains(a.id)).ToList(),"id","Transaction_type", income_Transaction.transaction_type_id);
            return View(income_Transaction);
        }

        // GET: Income_Transaction/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Income_Transaction income_Transaction = db.Income_Transaction.Find(id);
            if (income_Transaction == null)
            {
                return HttpNotFound();
            }
            ViewBag.custmor_id = new SelectList(db.Custmors, "id", "cusmor_name", income_Transaction.custmor_id);
            ViewBag.payment_type_id = new SelectList(db.Payment_Type, "id", "payment", income_Transaction.payment_type_id);
            ViewBag.suplier_type_id = new SelectList(db.SuplierTypePrices, "id", "nick",income_Transaction.suplier_type_id);
            //ViewBag.suplier_type_id = new SelectList(db.SuplierTypes, "id", "suplier_type", income_Transaction.suplier_type_id);
            ViewBag.transaction_type_id = new SelectList(db.TransactionTypes.Where(a => new[] { 1, 12 }.Contains(a.id)).ToList(), "id", "Transaction_type", income_Transaction.transaction_type_id);
            return View(income_Transaction);
        }

        // POST: Income_Transaction/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,transaction_date,transaction_type_id,amount,quantity,suplier_type_id,payment_type_id,custmor_id,note")] Income_Transaction income_Transaction,int id)
        {
            try
            {
                db.Entry(income_Transaction).State = EntityState.Modified;
                db.SaveChanges();
                TempData["AlertMessage"] = "edit";
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

            }
            ViewBag.custmor_id = new SelectList(db.Custmors, "id", "cusmor_name", income_Transaction.custmor_id);
            ViewBag.payment_type_id = new SelectList(db.Payment_Type, "id", "payment", income_Transaction.payment_type_id);
            ViewBag.suplier_type_id = new SelectList(db.SuplierTypePrices, "id", "nick", income_Transaction.suplier_type_id);
            //ViewBag.suplier_type_id = new SelectList(db.SuplierTypes, "id", "suplier_type", income_Transaction.suplier_type_id);
            ViewBag.transaction_type_id = new SelectList(db.TransactionTypes.Where(a => new[] { 1, 12 }.Contains(a.id)).ToList(), "id", "Transaction_type", income_Transaction.transaction_type_id);
            return View(income_Transaction);
        }

        // GET: Income_Transaction/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Income_Transaction income_Transaction = db.Income_Transaction.Find(id);
            if (income_Transaction == null)
            {
                return HttpNotFound();
            }
            ViewBag.custmor_id = new SelectList(db.Custmors, "id", "cusmor_name", income_Transaction.custmor_id);
            ViewBag.payment_type_id = new SelectList(db.Payment_Type, "id", "payment", income_Transaction.payment_type_id);
            ViewBag.suplier_type_id = new SelectList(db.SuplierTypePrices, "id", "nick", income_Transaction.suplier_type_id);
            ViewBag.transaction_type_id = new SelectList(db.TransactionTypes.Where(a => new[] { 1, 12 }.Contains(a.id)).ToList(), "id", "Transaction_type", income_Transaction.transaction_type_id);

            return View(income_Transaction);
        }

        // POST: Income_Transaction/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Income_Transaction income_Transaction = db.Income_Transaction.Find(id);
            db.Income_Transaction.Remove(income_Transaction);
            db.SaveChanges();

            ViewBag.custmor_id = new SelectList(db.Custmors, "id", "cusmor_name", income_Transaction.custmor_id);
            ViewBag.payment_type_id = new SelectList(db.Payment_Type, "id", "payment", income_Transaction.payment_type_id);
            ViewBag.suplier_type_id = new SelectList(db.SuplierTypePrices, "id", "nick", income_Transaction.suplier_type_id);
        
            ViewBag.transaction_type_id = new SelectList(db.TransactionTypes.Where(a => new[] { 1, 12 }.Contains(a.id)).ToList(), "id", "Transaction_type", income_Transaction.transaction_type_id);

            return RedirectToAction("Index");
        }
        public JsonResult GetPriceSuplierQuantity(int suplier_type_id)
        {
            using (var context = new WaterComponySystemEntities())
            {
                var price = context.SuplierTypePrices
                                   .Where(s => s.id == suplier_type_id)
                                   .Select(s => s.price)
                                   .SingleOrDefault();
                if (price == null)
                    price = 0;

                var suplier_Type_id = context.SuplierTypePrices
                                  .Where(s => s.id == suplier_type_id)
                                  .Select(s => s.suplier_Type_id)
                                  .SingleOrDefault();

                var suplier_type_id_all = context.SuplierTypes
                    .Where(s => s.id == suplier_Type_id)
                    .Select(s => s.suplier_type)
                    .SingleOrDefault();

                var QuantityStore = context.Warehouses
                                   .Where(s => s.suplier_type_id == suplier_type_id)
                                   .Select(S => S.quantity)
                                   .SingleOrDefault();

                if (QuantityStore == null)
                    QuantityStore = 0;


                return Json(new { price = price, suplier_type_id_all = suplier_type_id_all, quantity_in_Store = QuantityStore }, JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult GetCustmorLocation(int custmor_id)
        {
            // Assume you have a database context or repository to fetch data
            using (var context = new WaterComponySystemEntities())
            {
                var Custmor_Location = context.Custmors
                                   .Where(s => s.id == custmor_id)
                                   .Select(s => s.location_cus)
                                   .FirstOrDefault();

                return Json(new { Custmor_Location = Custmor_Location }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetTotal(int quantity, decimal price, decimal offer,int quantity_in_Store)
        {
            // Assume you have a database context or repository to fetch data
            decimal amount = (quantity * price) - offer;

            int quantityStore = quantity_in_Store - quantity;
                return Json(new { amount = amount , quantity_in_Store =quantityStore}, JsonRequestBehavior.AllowGet);
          
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
