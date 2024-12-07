using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.Mvc;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using WaterCompanySystem.Models;
using Microsoft.Ajax.Utilities;
using WaterCompanySystem.Reports;
using System.Configuration;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.EnterpriseServices.CompensatingResourceManager;

namespace WaterCompanySystem.Controllers
{
    public class WarehousesController : Controller
    {
        private WaterComponySystemEntities db = new WaterComponySystemEntities();

        public ActionResult Index3()
        {
            var warehouses = db.Warehouses
                               .Include(w => w.BottleType)
                               .Include(w => w.SuplierType)
                               .ToList();

            return View(warehouses);
        }

        ///////////////////////////////////////////////////////////////Display Store////////////////////////////////////////////////////////
        public JsonResult GetSuplierTypes()
        {
            var suplier = db.SuplierTypes.ToList();

            return Json(new {result=suplier,JsonRequestBehavior.AllowGet});
        }
        public ActionResult ReportParameterAmount()
        {

            return View();
        }
      
        public ActionResult GetStoreReportAmount(DateTime SearDate)
        {
            var repo = new ReportClass();
            repo.FileName = Server.MapPath("/Reports/RptTotalAmount.rpt");
            repo.SetParameterValue("Search_date", SearDate);
            //conexion para el reporte
            var coninfo = ReportConn.getConnection();
            TableLogOnInfo logoninfo = new TableLogOnInfo();
            Tables tables;
            tables = repo.Database.Tables;
            foreach (CrystalDecisions.CrystalReports.Engine.Table item in tables)
            {
                logoninfo = item.LogOnInfo;
                logoninfo.ConnectionInfo = coninfo;
                item.ApplyLogOnInfo(logoninfo);
            }
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            Stream stream = repo.ExportToStream(ExportFormatType.PortableDocFormat);
            return new FileStreamResult(stream, "application/pdf");
        }
        public ActionResult ReportParameter()
        {
            ViewBag.suplier_type_id = new SelectList(db.SuplierTypes, "id", "suplier_type");

            return View();
        }
        public ActionResult GetStoreReport(int supl_id)
        {
            var repo = new ReportClass();
            repo.FileName = Server.MapPath("/Reports/RptParameter.rpt");
            repo.SetParameterValue("suplier_type_id", supl_id);
            var coninfo = ReportConn.getConnection();
            TableLogOnInfo logoninfo = new TableLogOnInfo();
            Tables tables;
            tables = repo.Database.Tables;
            foreach (CrystalDecisions.CrystalReports.Engine.Table item in tables)
            {
                logoninfo = item.LogOnInfo;
                logoninfo.ConnectionInfo = coninfo;
                item.ApplyLogOnInfo(logoninfo);
            }
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            Stream stream = repo.ExportToStream(ExportFormatType.PortableDocFormat);
            return new FileStreamResult(stream, "application/pdf");
         
        }

        public ActionResult ReportParameterAll()
        {
            //ViewBag.suplier_type_id = new SelectList(db.SuplierTypes.DistinctBy(x => x.suplier_type), "id", "suplier_type");

            return View();
        }
        public ActionResult GetAllStoreReport()
        {
            var repo = new ReportClass();
            repo.FileName = Server.MapPath("/Reports/RptParameter.rpt");
          // repo.SetParameterValue("suplier_type_id", supl_id);
            //conexion para el reporte
            var coninfo = ReportConn.getConnection();
            TableLogOnInfo logoninfo = new TableLogOnInfo();
            Tables tables;
            tables = repo.Database.Tables;
            foreach (CrystalDecisions.CrystalReports.Engine.Table item in tables)
            {
                logoninfo = item.LogOnInfo;
                logoninfo.ConnectionInfo = coninfo;
                item.ApplyLogOnInfo(logoninfo);
            }
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            Stream stream = repo.ExportToStream(ExportFormatType.PortableDocFormat);


            return new FileStreamResult(stream, "application/pdf");
        }
        public ActionResult GetAllSuplierStore()
        {
            var query = from s in db.SuplierTypes
                        join w in db.Warehouses on s.id equals w.suplier_type_id into warehouseJoin
                        from wj in warehouseJoin.DefaultIfEmpty()
                        join e in db.Expense_Transaction on s.id equals e.suplier_type_id into expenseJoin
                        from ej in expenseJoin.DefaultIfEmpty()
                        join i in db.Income_Transaction on s.id equals i.suplier_type_id into incomeJoin
                        from ij in incomeJoin.DefaultIfEmpty()
                        group new { wj, ej, ij } by new { s.suplier_type, s.id } into grouped
                        select new
                        {
                            SuplierType = grouped.Key.suplier_type,
                            FirstQuantity = grouped.Sum(x => x.wj != null ? x.wj.quantity : 0),
                            TotalExpenseQuantity = grouped.Sum(x => x.ej != null ? x.ej.quantity : 0),
                            TotalIncomeQuantity = grouped.Sum(x => x.ij != null ? x.ij.quantity : 0),
                            StoreQuantity = grouped.Sum(x => (x.wj != null ? x.wj.quantity : 0) +
                                                             (x.ej != null ? x.ej.quantity : 0) -
                                                             (x.ij != null ? x.ij.quantity : 0))
                        };

            return View(query.ToList());
        }

        public ActionResult GetGetAllSuplierStoreReport()
        {
            var repo = new ReportClass();
            repo.FileName = Server.MapPath("/Reports/StoreReportSupp.rpt");
            var coninfo = ReportConn.getConnection();
            TableLogOnInfo logoninfo = new TableLogOnInfo();
            Tables tables;
            tables = repo.Database.Tables;
            foreach (CrystalDecisions.CrystalReports.Engine.Table item in tables)
            {
                logoninfo = item.LogOnInfo;
                logoninfo.ConnectionInfo = coninfo;
                item.ApplyLogOnInfo(logoninfo);
            }
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            Stream stream = repo.ExportToStream(ExportFormatType.PortableDocFormat);


            return new FileStreamResult(stream, "application/pdf");
        }

        public ActionResult ReportBetweenTotalAmount()
        {

            return View();
        }

        public ActionResult GetReportBetweenTotalAmount(DateTime? startDate, DateTime? endDate)
        {
            if (startDate == null || endDate == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Start_Date and End_Date are required.");
            }
            var repo = new ReportClass();
            
            repo.FileName = Server.MapPath("/Reports/RptBetweenTotalAmount.rpt");
            repo.SetParameterValue("Start_Date", startDate);
            repo.SetParameterValue("End_Date", endDate);
            //conexion para el reporte
            var coninfo = ReportConn.getConnection();
            TableLogOnInfo logoninfo = new TableLogOnInfo();
            Tables tables;
            tables = repo.Database.Tables;
            foreach (CrystalDecisions.CrystalReports.Engine.Table item in tables)
            {
                logoninfo = item.LogOnInfo;
                logoninfo.ConnectionInfo = coninfo;
                item.ApplyLogOnInfo(logoninfo);
            }
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            Stream stream = repo.ExportToStream(ExportFormatType.PortableDocFormat);


            return new FileStreamResult(stream, "application/pdf");
        }

        public ActionResult DisplayStore()
        {
            ViewBag.suplier_type_id = new SelectList(db.SuplierTypes, "id", "suplier_type");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DisplayStore(Warehouse ware)
        {

            ViewBag.suplier_type_id = new SelectList(db.SuplierTypes, "id", "suplier_type",ware.suplier_type_id);
            return View();
        }
        ///////////////////////////////////////////////////////////////Display Amount////////////////////////////////////////////////////////

        // GET: Warehouses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Warehouse warehouse = db.Warehouses.Find(id);
            if (warehouse == null)
            {
                return HttpNotFound();
            }
            return View(warehouse);
        }

        // GET: Warehouses/Create
        public ActionResult Create()
        {
            ViewBag.bottel_type_id = new SelectList(db.BottleTypes, "id", "bottle_type");
            ViewBag.suplier_type_id = new SelectList(db.SuplierTypes, "id", "suplier_type");
            return View();
        }

        // POST: Warehouses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,quantity,last_update,bottel_type_id,suplier_type_id")] Warehouse warehouse)
        {
            try
            {

                int x = db.Warehouses.Select(p => p.id).Cast<int?>().Max() ?? 0;
                int maxid = x + 1;
                warehouse.id = maxid;

                warehouse.last_update = System.DateTime.Now;
                warehouse.bottel_type_id = 1;

                db.Warehouses.Add(warehouse);
                db.SaveChanges();
                TempData["AlertMessage"] = "success";
                return RedirectToAction("Index3");
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
                return View(warehouse);
            }

            //ViewBag.bottel_type_id = new SelectList(db.BottleTypes, "id", "bottle_type", warehouse.bottel_type_id);
            ViewBag.suplier_type_id = new SelectList(db.SuplierTypes, "id", "suplier_type",warehouse.suplier_type_id);
            return View(warehouse);
        }

        // GET: Warehouses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Warehouse warehouse = db.Warehouses.Find(id);
            if (warehouse == null)
            {
                return HttpNotFound();
            }
            ViewBag.bottel_type_id = new SelectList(db.BottleTypes, "id", "bottle_type", warehouse.bottel_type_id);
            ViewBag.suplier_type_id = new SelectList(db.SuplierTypes, "id", "suplier_type", warehouse.suplier_type_id);
            return View(warehouse);
        }

        // POST: Warehouses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,quantity,last_update,bottel_type_id,suplier_type_id")] Warehouse warehouse)
        {
           try
            {
                db.Entry(warehouse).State = EntityState.Modified;
                db.SaveChanges();
                TempData["AlertMessage"] = "edit";
                return RedirectToAction("Index3");
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
            ViewBag.bottel_type_id = new SelectList(db.BottleTypes, "id", "bottle_type", warehouse.bottel_type_id);
            ViewBag.suplier_type_id = new SelectList(db.SuplierTypes, "id", "suplier_type", warehouse.suplier_type_id);
            return View(warehouse);
        }

        // GET: Warehouses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Warehouse warehouse = db.Warehouses.Find(id);
            if (warehouse == null)
            {
                return HttpNotFound();
            }
            return View(warehouse);
        }

        // POST: Warehouses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Warehouse warehouse = db.Warehouses.Find(id);
            db.Warehouses.Remove(warehouse);
            db.SaveChanges();
            return RedirectToAction("Index3");
        }

       
        public ActionResult DisplayAmount()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DisplayAmount(Warehouse ware)
        {
            return View();
        }
        public JsonResult GetAmountTotal(DateTime Search_date)
        {
            // Assume you have a database context or repository to fetch data
            using (var context = new WaterComponySystemEntities())
            {

                var Expence_Rev_Amount = context.Expense_Transaction
                                    .Where(s => DbFunctions.TruncateTime(s.transaction_date) == Search_date.Date && s.transaction_type_id == 12)
                                    .Sum(s => s.amount);
                if (Expence_Rev_Amount == null)
                    Expence_Rev_Amount = 0;

                var Expence_Dis_Amount = context.Income_Transaction
                               .Where(s => DbFunctions.TruncateTime(s.transaction_date) == Search_date.Date && new[] { 2, 3, 4, 5, 6, 7, 8, 9, 10 }.Contains(s.transaction_type_id))
                               .Sum(s => s.amount);
                if (Expence_Dis_Amount == null)
                    Expence_Dis_Amount = 0;

                var Income_Sum_1_Rev = context.Income_Transaction
                             .Where(s => DbFunctions.TruncateTime(s.transaction_date) == Search_date.Date && s.transaction_type_id == 1)
                             .Sum(s => s.amount);
                if (Income_Sum_1_Rev == null)
                    Income_Sum_1_Rev = 0;

                var Income_Sum_11_Debts = context.Income_Transaction
                                .Where(s => DbFunctions.TruncateTime(s.transaction_date) == Search_date.Date && s.transaction_type_id==11)
                                .Sum(s => s.amount);
                if (Income_Sum_11_Debts == null)
                    Income_Sum_11_Debts = 0;

                var Income_Amount = Income_Sum_1_Rev - Income_Sum_11_Debts;
                if (Income_Amount == null)
                    Income_Amount = 0;

                var TotalAmount = Income_Amount - Expence_Rev_Amount - Expence_Dis_Amount;

                if (TotalAmount == null)
                    TotalAmount = 0;

                return Json(new { Expence_Dis_Amount= Expence_Dis_Amount,Expence_Rev_Amount = Expence_Rev_Amount, Income_Amount = Income_Amount, NetAmount = TotalAmount }, JsonRequestBehavior.AllowGet);
            }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public JsonResult GetStoreInfo(int suplier_type_id)
        {
            // Assume you have a database context or repository to fetch data
            using (var context = new WaterComponySystemEntities())
            {
                var Firstquantity = context.Warehouses
                       .Where(x => x.suplier_type_id == suplier_type_id) // Apply the filter
                       .Sum(x => x.quantity);

                if (Firstquantity == null)
                    Firstquantity = 0;

                var result_Expense = context.Expense_Transaction
                                   .Where(s => s.suplier_type_id == suplier_type_id)
                                   .Sum(s => s.quantity);
                if (result_Expense == null)
                    result_Expense = 0;

                var syplier_type = context.SuplierTypePrices
                    .Where(s => s.suplier_Type_id == suplier_type_id)
                    .Select(s => s.id).ToList();
                ;
                var result_income = context.Income_Transaction
                     .Where(s => syplier_type.Contains((int)s.suplier_type_id))
                     .Sum(s => s.quantity);

                if (result_income == null)
                    result_income = 0;

                var StoreQuantity = (Firstquantity + result_Expense) - result_income;

                return Json(new { StoreQuantity = StoreQuantity, quantity = Firstquantity }, JsonRequestBehavior.AllowGet);
            }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public ActionResult GetStoreInfoBySyplierDataReportIndex()
        {
            ViewBag.suplier_type_id = new SelectList(db.SuplierTypes, "id", "suplier_type");
            return View();
        }
        /// <summary>
        /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        public JsonResult GetStoreInfoBySyplierDataReport(int? suplier_type_id)
        {
            if (suplier_type_id == null)
            {
                throw new ArgumentNullException(nameof(suplier_type_id), "suplier_type_id is required.");
            }
            var dt = GetStoreInfoBySyplierData(suplier_type_id);

            if (dt != null /*&& dt..Rows != null && dt.Rows.Count != 0*/)
            {
                string strReportName = "StoreInfoBySyplierDataReport";
                using (ReportDocument rd = new ReportDocument())
                {
                    Session["ReportData"] = dt;
                    Session["ReportOption"] = "Reports";
                    Session["reportname"] = strReportName;

                    Tools tool = new Tools();
                    strReportName = Session["reportname"].ToString();
                    string strRptPath = tool.getProgectPath() + strReportName + ".rpt";

                    if (Session["ReportData"] != null)
                    {
                        var ReportData = Session["ReportData"];

                        rd.Load(strRptPath);
                        rd.SetParameterValue("suplier_type_id", suplier_type_id);

                        rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat,
                        System.Web.HttpContext.Current.Response, false, "Report");

                        // rd.SetDataSource(dt);

                        //  List<KeyValuePair<string, string>> reportParameters = (List<KeyValuePair<string, string>>)Session["ReportParameter"];
                        //  if (reportParameters != null)
                        //  {

                        //      foreach (var reportParameter in reportParameters)
                        //      {
                        //          rd.SetParameterValue("@" + reportParameter.Key, reportParameter.Value);
                        //      }
                        //  }
                        ////  rd.SetDatabaseLogon("medicaltreatment", "medicaltreatment");
                        //  var isWord = Session["ReportType"];
                        //  if (isWord != null)
                        //      isWord = isWord.ToString();
                        //  if (isWord == "word")
                        //      rd.ExportToHttpResponse(ExportFormatType.WordForWindows, System.Web.HttpContext.Current.Response, false, "Report");
                        //  else if (isWord == "excel")
                        //      rd.ExportToHttpResponse(ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, "Report");
                        //  else
                        //      rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Report");
                    }

                    else
                    {
                        Response.Write("<H2>Nothing Found, No Data To Show</H2>");
                    }
                }


                //List<KeyValuePair<string, string>> param = new List<KeyValuePair<string, string>>();


                return Json(true, JsonRequestBehavior.AllowGet);

            }

            else
            {
                var mag = "";
                mag = "لا يوجد بيانات لعرضها";
                return Json(mag, JsonRequestBehavior.AllowGet);
            }
        }
        private async Task<DataTable> GetStoreInfoBySyplierData(int? suplier_type_id)
        {
            if (suplier_type_id == null)
            {
                throw new ArgumentNullException(nameof(suplier_type_id), "suplier_type_id is required.");
            }

            //var sqlQuery = String.Format("SELECT(ISNULL((SELECT SUM(w.quantity) FROM Warehouse w WHERE e.suplier_type_id ="+ suplier_type_id + "),0) - ISNULL((SELECT SUM(i.quantity) FROM Income_Transaction i WHERE i.suplier_type_id IN (SELECT st.id FROM SuplierTypePrice st WHERE st.suplier_Type_id = "+ suplier_type_id + ") ), 0)) AS StoreQuantity, ISNULL( (SELECT SUM(w.quantity) FROM Warehouse w WHERE w.suplier_type_id = "+ suplier_type_id + "), 0) AS Firstquantity , ISNULL((SELECT SUM(e.quantity) FROM Expense_Transaction e  WHERE e.suplier_type_id = "+ suplier_type_id + "), 0) AS Expense");
            //var sqlQuery = String.Format("SELECT (Select suplier_type from SuplierType s where s.id=\"+suplier_type_id+\"),ISNULL(SUM(w.quantity), 0) AS FirstQuantity, ISNULL((SELECT SUM(e.quantity) FROM Expense_Transaction e WHERE e.suplier_type_id = \"+suplier_type_id+\"), 0) AS TotalExpenseQuantity,ISNULL((SELECT SUM(i.quantity) FROM Income_Transaction i WHERE i.suplier_type_id = \"+suplier_type_id+\"), 0) AS TotalIncomeQuantity,(ISNULL(SUM(w.quantity), 0) + ISNULL((SELECT SUM(e.quantity) FROM Expense_Transaction e WHERE e.suplier_type_id = \"+suplier_type_id+\"), 0) - ISNULL((SELECT SUM(i.quantity) FROM Income_Transaction i WHERE i.suplier_type_id = \"+suplier_type_id+\"), 0)) AS StoreQuantity FROM Warehouse w WHERE w.suplier_type_id = \"+suplier_type_id+\")");
            // var sqlQuery = String.Format(
            //"SELECT " +
            //"(SELECT suplier_type FROM SuplierType s WHERE s.id = {0}) AS suplier_type, " +
            //"ISNULL(SUM(w.quantity), 0) AS FirstQuantity, " +
            //"ISNULL((SELECT SUM(e.quantity) FROM Expense_Transaction e WHERE e.suplier_type_id = {0}), 0) AS TotalExpenseQuantity, " +
            //"ISNULL((SELECT SUM(i.quantity) FROM Income_Transaction i WHERE i.suplier_type_id = {0}), 0) AS TotalIncomeQuantity, " +
            //"(ISNULL(SUM(w.quantity), 0) + " +
            //"ISNULL((SELECT SUM(e.quantity) FROM Expense_Transaction e WHERE e.suplier_type_id = {0}), 0) - " +
            //"ISNULL((SELECT SUM(i.quantity) FROM Income_Transaction i WHERE i.suplier_type_id = {0}), 0)) AS StoreQuantity " +
            //"FROM Warehouse w " +
            //"WHERE w.suplier_type_id = {0}", suplier_type_id);

            var sqlQuery = String.Format(@"
    SELECT 
    (SELECT suplier_type from SuplierType sss where sss.id = {0}),
        ISNULL(SUM(w.quantity), 0) AS FirstQuantity,
        ISNULL(
            (SELECT SUM(e.quantity)
             FROM Expense_Transaction e
             WHERE e.suplier_type_id = {0}), 0) AS TotalExpenseQuantity,
        ISNULL(
            (SELECT SUM(i.quantity)
             FROM Income_Transaction i
             WHERE i.suplier_type_id IN (
                SELECT st.id 
                FROM SuplierTypePrice st 
                WHERE st.suplier_Type_id = {0})
            ), 0) AS TotalIncomeQuantity,
        (
            ISNULL(SUM(w.quantity), 0) + 
            ISNULL((SELECT SUM(e.quantity) 
                    FROM Expense_Transaction e 
                    WHERE e.suplier_type_id = {0}), 0) 
            - 
            ISNULL((SELECT SUM(i.quantity) 
                    FROM Income_Transaction i 
                    WHERE i.suplier_type_id IN (
                        SELECT st.id 
                        FROM SuplierTypePrice st 
                        WHERE st.suplier_Type_id = {0})
                   ), 0)
        ) AS StoreQuantity
    FROM 
        Warehouse w
    WHERE 
        w.suplier_type_id = {0};
", suplier_type_id);

            DataTable dt = await ExecuteSql(sqlQuery);
            return dt;
        }
        public static async Task<DataTable> ExecuteSql(string sql)
        {
            string efConnectionString = ConfigurationManager.ConnectionStrings["WaterComponySystemEntities"].ConnectionString;
            var entityBuilder = new EntityConnectionStringBuilder(efConnectionString);
            string sqlConnectionString = entityBuilder.ProviderConnectionString;
            var strcon = new SqlConnectionStringBuilder(sqlConnectionString);

            //var coninfo = ReportConn.getConnection();
            using (SqlConnection SQLConn = new SqlConnection(sqlConnectionString))
            {
                SQLConn.Open();

                SqlCommand Cmd = new SqlCommand();
                DataTable dt = new DataTable();

                Cmd.Connection = SQLConn;
                Cmd.CommandType = CommandType.Text;
                Cmd.CommandText = sql;

                using (SqlDataReader rdr = Cmd.ExecuteReader())
                {
                    dt.Load(rdr);
                }
                SQLConn.Close();
                return dt;
            }

        }


        ///////////////////////////////////////Display All Suplaier///////////////////////////////////////////////
        public JsonResult GetAllStoreInfoBySyplierDataReport()
        {
          
            var dt = GetAllStoreInfoBySyplierData();

            if (dt != null /*&& dt..Rows != null && dt.Rows.Count != 0*/)
            {
                string strReportName = "AllStoreInfoBySyplierDataReport";
                using (ReportDocument rd = new ReportDocument())
                {
                    Session["ReportData"] = dt;
                    Session["ReportOption"] = "Reports";
                    Session["reportname"] = strReportName;

                    Tools tool = new Tools();
                    strReportName = Session["reportname"].ToString();
                    string strRptPath = tool.getProgectPath() + strReportName + ".rpt";

                    if (Session["ReportData"] != null)
                    {
                        var ReportData = Session["ReportData"];
                        rd.Load(strRptPath);
                        rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat,
                        System.Web.HttpContext.Current.Response, false, "Report");
                     }
                    else
                    {
                        Response.Write("<H2>Nothing Found, No Data To Show</H2>");
                    }
                }
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            else
            {
                var mag = "";
                mag = "لا يوجد بيانات لعرضها";
                return Json(mag, JsonRequestBehavior.AllowGet);
            }
        }
        private async Task<DataTable> GetAllStoreInfoBySyplierData()
        {
            var sqlQuery = String.Format(@"SELECT 
    (SELECT suplier_type 
     FROM SuplierType sss 
     WHERE sss.id = w.suplier_type_id) AS SuplierType,
    ISNULL(SUM(w.quantity), 0) AS FirstQuantity,
    ISNULL(
        (SELECT SUM(e.quantity)
         FROM Expense_Transaction e
         WHERE e.suplier_type_id = w.suplier_type_id), 0) AS TotalExpenseQuantity,
    ISNULL(
        (SELECT SUM(i.quantity)
         FROM Income_Transaction i
         WHERE i.suplier_type_id IN (
            SELECT st.id 
            FROM SuplierTypePrice st 
            WHERE st.suplier_Type_id = w.suplier_type_id)
        ), 0) AS TotalIncomeQuantity,
    (
        ISNULL(SUM(w.quantity), 0) + 
        ISNULL((SELECT SUM(e.quantity) 
                FROM Expense_Transaction e 
                WHERE e.suplier_type_id = w.suplier_type_id), 0) 
        - 
        ISNULL((SELECT SUM(i.quantity) 
                FROM Income_Transaction i 
                WHERE i.suplier_type_id IN (
                    SELECT st.id 
                    FROM SuplierTypePrice st 
                    WHERE st.suplier_Type_id = w.suplier_type_id)
               ), 0)
    ) AS StoreQuantity
FROM 
    Warehouse w
GROUP BY 
    w.suplier_type_id;
");
            DataTable dt = await ExecuteSql(sqlQuery);
            return dt;
        }


        /////////////////////////////////////////display by Date/////////////////////////////////////////////////////
     
        public JsonResult GetStoreInfoBySearch_dateReport(DateTime Search_date)
        {
            if (Search_date == null)
            {
                throw new ArgumentNullException(nameof(Search_date), "Search_date is required.");
            }
            var dt = GetStoreInfoBySearch_date(Search_date);

            if (dt != null /*&& dt..Rows != null && dt.Rows.Count != 0*/)
            {
                string strReportName = "RptTotalAmount";
                using (ReportDocument rd = new ReportDocument())
                {
                    Session["ReportData"] = dt;
                    Session["ReportOption"] = "Reports";
                    Session["reportname"] = strReportName;

                    Tools tool = new Tools();
                    strReportName = Session["reportname"].ToString();
                    string strRptPath = tool.getProgectPath() + strReportName + ".rpt";

                    if (Session["ReportData"] != null)
                    {
                        var ReportData = Session["ReportData"];

                        rd.Load(strRptPath);
                        rd.SetParameterValue("Search_date", Search_date);

                        rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat,
                        System.Web.HttpContext.Current.Response, false, "Report");
                       }
                    else
                    {
                        Response.Write("<H2>Nothing Found, No Data To Show</H2>");
                    }
                }
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            else
            {
                var mag = "";
                mag = "لا يوجد بيانات لعرضها";
                return Json(mag, JsonRequestBehavior.AllowGet);
            }
        }
        private async Task<DataTable> GetStoreInfoBySearch_date(DateTime Search_date)
        {
            if (Search_date == null)
            {
                throw new ArgumentNullException(nameof(Search_date), "Search_date is required.");
            }

            var sqlQuery = String.Format("SELECT ISNULL( (SELECT SUM(e.amount) FROM Expense_Transaction e  WHERE e.transaction_type_id = 13  AND CONVERT(DATE, e.transaction_date) = +'Search_date'+), 0) AS Expence_Rev_Amount_mordon, ISNULL((SELECT SUM(i.amount) FROM Expense_Transaction i  WHERE i.transaction_type_id IN (2, 3, 4, 5, 6, 7, 8, 9, 10,11) AND CONVERT(DATE, i.transaction_date) =+'Search_date'+ ), 0) AS Expence_Dis_Amount_monsrfat, ISNULL((SELECT SUM(i.amount) FROM Income_Transaction i WHERE i.transaction_type_id = 1 AND CONVERT(DATE, i.transaction_date) = +'Search_date'+ ), 0) AS Income_Sum_1_Rev_eradat, ISNULL( (SELECT SUM(i.amount) FROM Income_Transaction i  WHERE i.transaction_type_id = 12  AND CONVERT(DATE, i.transaction_date) =+'Search_date'+ ), 0) AS Income_Sum_11_Debts_duon, (ISNULL((SELECT SUM(i.amount)  FROM Income_Transaction i  WHERE i.transaction_type_id = 1  AND CONVERT(DATE, i.transaction_date) =+'Search_date'+ ), 0) - ISNULL( (SELECT SUM(i.amount)   FROM Income_Transaction i   WHERE i.transaction_type_id = 12   AND CONVERT(DATE, i.transaction_date) =+'Search_date'+ ), 0)) AS Income_Amount_safeEradat, (ISNULL((SELECT SUM(i.amount)  FROM Income_Transaction i  WHERE i.transaction_type_id = 1  AND CONVERT(DATE, i.transaction_date) =+'Search_date'+ ), 0)    - ISNULL(  (SELECT SUM(i.amount)    FROM Income_Transaction i    WHERE i.transaction_type_id = 12    AND CONVERT(DATE, i.transaction_date) =+'Search_date'+ ), 0)  - ISNULL( (SELECT SUM(e.amount)   FROM Expense_Transaction e   WHERE e.transaction_type_id = 13   AND CONVERT(DATE, e.transaction_date) =+'Search_date'+ ), 0)   - ISNULL( (SELECT SUM(i.amount)   FROM Expense_Transaction i   WHERE i.transaction_type_id IN (2, 3, 4, 5, 6, 7, 8, 9, 10,11)   AND CONVERT(DATE, i.transaction_date) =+'Search_date'+ ), 0)) AS NetAmount;", Search_date);

            DataTable dt = await ExecuteSql(sqlQuery);
            return dt;
        }

        /////////////////////////////////////////display by 2 Date/////////////////////////////////////////////////////

        public JsonResult GetStoreInfoByTowSearch_dateReport(DateTime startDate1, DateTime endDate1)
        {
            if (startDate1 == null || endDate1 == null)
            {
                throw new ArgumentNullException(nameof(startDate1), "startDate is required.");
            }
           
            var dt = GetStoreInfoByTowSearch_date(startDate1, endDate1);

            if (dt != null /*&& dt..Rows != null && dt.Rows.Count != 0*/)
            {
                string strReportName = "RptBetweenTotalAmount";
                using (ReportDocument rd = new ReportDocument())
                {
                    Session["ReportData"] = dt;
                    Session["ReportOption"] = "Reports";
                    Session["reportname"] = strReportName;

                    Tools tool = new Tools();
                    strReportName = Session["reportname"].ToString();
                    string strRptPath = tool.getProgectPath() + strReportName + ".rpt";

                    if (Session["ReportData"] != null)
                    {
                        var ReportData = Session["ReportData"];

                        rd.Load(strRptPath);
                        rd.SetParameterValue("start_Date", startDate1);
                        rd.SetParameterValue("end_Date", endDate1);

                        rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat,
                        System.Web.HttpContext.Current.Response, false, "Report");
                    }
                    else
                    {
                        Response.Write("<H2>Nothing Found, No Data To Show</H2>");
                    }
                }
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            else
            {
                var mag = "";
                mag = "لا يوجد بيانات لعرضها";
                return Json(mag, JsonRequestBehavior.AllowGet);
            }
        }
        private async Task<DataTable> GetStoreInfoByTowSearch_date(DateTime startDate1, DateTime endDate1)
        {
            if (startDate1 == null)
            {
                throw new ArgumentNullException(nameof(startDate1), "startDate is required.");
            }

            var startDate = startDate1.ToString("yyyy-MM-dd");
            var endDate = endDate1.ToString("yyyy-MM-dd");

            var sqlQuery = String.Format(" SELECT      ISNULL(         (SELECT SUM(e.amount)           FROM Expense_Transaction e            WHERE e.transaction_type_id = 13            AND CONVERT(DATE, e.transaction_date) BETWEEN '"+ startDate + "' AND '"+endDate+"'), 0) AS Expence_Rev_Amount_mordon,     ISNULL(         (SELECT SUM(i.amount)           FROM Expense_Transaction i            WHERE i.transaction_type_id IN (2, 3, 4, 5, 6, 7, 8, 9, 10, 11)           AND CONVERT(DATE, i.transaction_date) BETWEEN '"+ startDate+ "' AND '"+endDate+"'), 0) AS Expence_Dis_Amount_monsrfat,     ISNULL(         (SELECT SUM(i.amount)           FROM Income_Transaction i           WHERE i.transaction_type_id = 1           AND CONVERT(DATE, i.transaction_date) BETWEEN '"+ startDate+ "' AND '"+endDate+"'), 0) AS Income_Sum_1_Rev_eradat,     ISNULL(         (SELECT SUM(i.amount)           FROM Income_Transaction i            WHERE i.transaction_type_id = 12            AND CONVERT(DATE, i.transaction_date) BETWEEN '"+ startDate+ "' AND '"+endDate+"'), 0) AS Income_Sum_11_Debts_duon,     (ISNULL(         (SELECT SUM(i.amount)           FROM Income_Transaction i            WHERE i.transaction_type_id = 1           AND CONVERT(DATE, i.transaction_date) BETWEEN '"+ startDate+ "' AND '"+endDate+"'), 0)       - ISNULL(          (SELECT SUM(i.amount)            FROM Income_Transaction i            WHERE i.transaction_type_id = 12            AND CONVERT(DATE, i.transaction_date) BETWEEN '"+ startDate+ "' AND '"+endDate+"'), 0)) AS Income_Amount_safeEradat,     (ISNULL(         (SELECT SUM(i.amount)           FROM Income_Transaction i            WHERE i.transaction_type_id = 1            AND CONVERT(DATE, i.transaction_date) BETWEEN '"+ startDate+ "' AND '"+endDate+"'), 0)       - ISNULL(          (SELECT SUM(i.amount)            FROM Income_Transaction i             WHERE i.transaction_type_id = 12             AND CONVERT(DATE, i.transaction_date) BETWEEN '"+ startDate+ "' AND '"+endDate+"'), 0)      - ISNULL(          (SELECT SUM(e.amount)            FROM Expense_Transaction e             WHERE e.transaction_type_id = 13             AND CONVERT(DATE, e.transaction_date) BETWEEN '"+ startDate+ "' AND '"+endDate+"'), 0)       - ISNULL(          (SELECT SUM(i.amount)            FROM Expense_Transaction i             WHERE i.transaction_type_id IN (2, 3, 4, 5, 6, 7, 8, 9, 10, 11)            AND CONVERT(DATE, i.transaction_date) BETWEEN '"+ startDate+ "' AND '"+endDate+"'), 0)) AS NetAmount");
               // "           //var sqlQuery = String.Format("SELECT ISNULL( (SELECT SUM(e.amount) FROM Expense_Transaction e  WHERE e.transaction_type_id = 13 AND CONVERT(DATE, e.transaction_date) BETWEEN +'start_date'+ AND +'end_date'+), 0) AS Expence_Rev_Amount_mordon,    ISNULL(  (SELECT SUM(i.amount)      FROM Expense_Transaction i      WHERE i.transaction_type_id IN (2, 3, 4, 5, 6, 7, 8, 9, 10, 11)      AND CONVERT(DATE, i.transaction_date) BETWEEN +'start_date'+ AND +'end_date'+), 0) AS Expence_Dis_Amount_monsrfat,  ISNULL(  (SELECT SUM(i.amount) FROM Income_Transaction i      WHERE i.transaction_type_id = 1  AND CONVERT(DATE, i.transaction_date) BETWEEN +'start_date'+ AND +'end_date'+), 0) AS Income_Sum_1_Rev_eradat,    ISNULL(  (SELECT SUM(i.amount)      FROM Income_Transaction i      WHERE i.transaction_type_id = 12      AND CONVERT(DATE, i.transaction_date) BETWEEN +'start_date'+ AND +'end_date'+), 0) AS Income_Sum_11_Debts_duon,    (ISNULL(  (SELECT SUM(i.amount)      FROM Income_Transaction i      WHERE i.transaction_type_id = 1      AND CONVERT(DATE, i.transaction_date) BETWEEN +'start_date'+ AND +'end_date'+), 0)  - ISNULL(   (SELECT SUM(i.amount)       FROM Income_Transaction i       WHERE i.transaction_type_id = 12       AND CONVERT(DATE, i.transaction_date) BETWEEN +'start_date'+ AND +'end_date'+), 0)) AS Income_Amount_safeEradat,    (ISNULL(  (SELECT SUM(i.amount)      FROM Income_Transaction i      WHERE i.transaction_type_id = 1      AND CONVERT(DATE, i.transaction_date) BETWEEN +'start_date'+ AND +'end_date'+), 0)  - ISNULL(   (SELECT SUM(i.amount)       FROM Income_Transaction i       WHERE i.transaction_type_id = 12       AND CONVERT(DATE, i.transaction_date) BETWEEN +'start_date'+ AND +'end_date'+), 0)     - ISNULL(   (SELECT SUM(e.amount)       FROM Expense_Transaction e       WHERE e.transaction_type_id = 13       AND CONVERT(DATE, e.transaction_date) BETWEEN +'start_date'+ AND +'end_date'+), 0)  - ISNULL(   (SELECT SUM(i.amount)       FROM Expense_Transaction i       WHERE i.transaction_type_id IN (2, 3, 4, 5, 6, 7, 8, 9, 10, 11)       AND CONVERT(DATE, i.transaction_date) BETWEEN +'start_date'+ AND +'end_date'+), 0)) AS NetAmount", startDate, endDate);
    //        var sqlQuery = String.Format(@"
    //SELECT 
    //    ISNULL(
    //        (SELECT SUM(e.amount) 
    //         FROM Expense_Transaction e  
    //         WHERE e.transaction_type_id = 13  
    //         AND CONVERT(DATE, e.transaction_date) BETWEEN '{0}' AND '{1}'), 0) AS Expence_Rev_Amount_mordon,

    //    ISNULL(
    //        (SELECT SUM(i.amount) 
    //         FROM Expense_Transaction i  
    //         WHERE i.transaction_type_id IN (2, 3, 4, 5, 6, 7, 8, 9, 10, 11) 
    //         AND CONVERT(DATE, i.transaction_date) BETWEEN '{0}' AND '{1}'), 0) AS Expence_Dis_Amount_monsrfat,

    //    ISNULL(
    //        (SELECT SUM(i.amount) 
    //         FROM Income_Transaction i 
    //         WHERE i.transaction_type_id = 1 
    //         AND CONVERT(DATE, i.transaction_date) BETWEEN '{0}' AND '{1}'), 0) AS Income_Sum_1_Rev_eradat,

    //    ISNULL(
    //        (SELECT SUM(i.amount) 
    //         FROM Income_Transaction i  
    //         WHERE i.transaction_type_id = 12  
    //         AND CONVERT(DATE, i.transaction_date) BETWEEN '{0}' AND '{1}'), 0) AS Income_Sum_11_Debts_duon,

    //    (ISNULL(
    //        (SELECT SUM(i.amount) 
    //         FROM Income_Transaction i  
    //         WHERE i.transaction_type_id = 1 
    //         AND CONVERT(DATE, i.transaction_date) BETWEEN '{0}' AND '{1}'), 0) 
    //     - ISNULL(
    //         (SELECT SUM(i.amount) 
    //          FROM Income_Transaction i 
    //          WHERE i.transaction_type_id = 12 
    //          AND CONVERT(DATE, i.transaction_date) BETWEEN '{0}' AND '{1}'), 0)) AS Income_Amount_safeEradat,

    //    (ISNULL(
    //        (SELECT SUM(i.amount) 
    //         FROM Income_Transaction i  
    //         WHERE i.transaction_type_id = 1  
    //         AND CONVERT(DATE, i.transaction_date) BETWEEN '{0}' AND '{1}'), 0) 
    //     - ISNULL(
    //         (SELECT SUM(i.amount) 
    //          FROM Income_Transaction i  
    //          WHERE i.transaction_type_id = 12  
    //          AND CONVERT(DATE, i.transaction_date) BETWEEN '{0}' AND '{1}'), 0)
    //     - ISNULL(
    //         (SELECT SUM(e.amount) 
    //          FROM Expense_Transaction e  
    //          WHERE e.transaction_type_id = 13  
    //          AND CONVERT(DATE, e.transaction_date) BETWEEN '{0}' AND '{1}'), 0) 
    //     - ISNULL(
    //         (SELECT SUM(i.amount) 
    //          FROM Expense_Transaction i  
    //          WHERE i.transaction_type_id IN (2, 3, 4, 5, 6, 7, 8, 9, 10, 11) 
    //          AND CONVERT(DATE, i.transaction_date) BETWEEN '{0}' AND '{1}'), 0)) AS NetAmount;",
    //startDate, endDate);

            DataTable dt = await ExecuteSql2Date(sqlQuery, startDate1, endDate1);
            return dt;
        }

        public static async Task<DataTable> ExecuteSql2Date(string sql, DateTime startDate1, DateTime endDate1)
        {
            string efConnectionString = ConfigurationManager.ConnectionStrings["WaterComponySystemEntities"].ConnectionString;
            var entityBuilder = new EntityConnectionStringBuilder(efConnectionString);
            string sqlConnectionString = entityBuilder.ProviderConnectionString;
            var strcon = new SqlConnectionStringBuilder(sqlConnectionString);

            //var coninfo = ReportConn.getConnection();
            using (SqlConnection SQLConn = new SqlConnection(sqlConnectionString))
            {
                SQLConn.Open();

                SqlCommand Cmd = new SqlCommand();
                DataTable dt = new DataTable();

                Cmd.Connection = SQLConn;
                Cmd.CommandType = CommandType.Text;
                Cmd.CommandText = sql;
                Cmd.Parameters.AddWithValue("start_Date", startDate1);
                Cmd.Parameters.AddWithValue("end_Date", endDate1);

                using (SqlDataReader rdr = Cmd.ExecuteReader())
                {
                    dt.Load(rdr);
                }
                SQLConn.Close();
                return dt;
            }

        }

    }
}
