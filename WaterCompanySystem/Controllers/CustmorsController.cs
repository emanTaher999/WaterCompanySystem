using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.ReportAppServer;
using CrystalDecisions.Shared;
using WaterCompanySystem.Models;
using WaterCompanySystem.Reports;

namespace WaterCompanySystem.Controllers
{
    public class CustmorsController : Controller
    {
        private WaterComponySystemEntities db = new WaterComponySystemEntities();

        // GET: Custmors
        public ActionResult Index()
        {
            var custmors = db.Custmors.Include(c => c.BottleType);
            return View(custmors.ToList());
        }
        public ActionResult IndexReport()
        {
            var custmors = db.Custmors.Include(c => c.BottleType);
            return View(custmors.ToList());
        }
        public ActionResult Export()
        {
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports/CustmorReport.rpt")));
            rd.SetDataSource(db.Custmors.Select(p => new
            {
                Id = p.id,
                Name = p.cusmor_name,
                Location = p.location_cus,
                Contact_number = p.contact_number,
                Email=p.email,
                Bottel_type_id = p.bottel_type_id
            }).ToList());
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            Stream stream = rd.ExportToStream
            (CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/pdf", "ListCustmors.pdf");
          
        }

        // GET: Custmors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Custmor custmor = db.Custmors.Find(id);
            if (custmor == null)
            {
                return HttpNotFound();
            }
            return View(custmor);
        }

        // GET: Custmors/Create
        public ActionResult Create()
        {
            ViewBag.bottel_type_id = new SelectList(db.BottleTypes,"id", "bottle_type");
            return View();
        }

        // POST: Custmors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,cusmor_name,location_cus,contact_number,email,number_of_bottel,bottel_type_id,creat_at")] Custmor custmor)
        {
            try
            {
                int x = db.Custmors.Select(p => p.id).Cast<int?>().Max() ?? 0;
                int maxid = x + 1;
                custmor.id = maxid;

                if (custmor.number_of_bottel == null)
                    custmor.number_of_bottel = 0;

                custmor.creat_at = System.DateTime.Now;
                db.Custmors.Add(custmor);
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
                return View(custmor);
            }
            ViewBag.bottel_type_id = new SelectList(db.BottleTypes, "id", "bottel_type_id", custmor.bottel_type_id);
            return View(custmor);
        }

        // GET: Custmors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Custmor custmor = db.Custmors.Find(id);
            if (custmor == null)
            {
                return HttpNotFound();
            }
            ViewBag.bottel_type_id = new SelectList(db.BottleTypes, "id", "bottle_type", custmor.bottel_type_id);
            return View(custmor);
        }

        // POST: Custmors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,cusmor_name,location_cus,contact_number,email,number_of_bottel,bottel_type_id,creat_at")] Custmor custmor)
        {
            try
            {
                db.Entry(custmor).State = EntityState.Modified;
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
            ViewBag.bottel_type_id = new SelectList(db.BottleTypes, "id", "bottle_type", custmor.bottel_type_id);
            return View(custmor);
        }

        // GET: Custmors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Custmor custmor = db.Custmors.Find(id);
            if (custmor == null)
            {
                return HttpNotFound();
            }
            return View(custmor);
        }

        // POST: Custmors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Custmor custmor = db.Custmors.Find(id);
            db.Custmors.Remove(custmor);
            db.SaveChanges();
            TempData["AlertMessage"] = "deleted";
            return RedirectToAction("Index");
        }

        public ActionResult CustmorReport()
        {
            try
            {
                var repo = new ReportClass();
                repo.FileName = Server.MapPath("/Reports/CustmorReport_AR.rpt");
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
                return View();
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


        //////////////////////////////////////////////////////////////////////////////////////
        public async Task<ActionResult> CustmorDataData()
        {
            return View();
        }
        public JsonResult GetCustmorDataReport()
        {
            var dt = GetCustmorData();

            if (dt != null /*&& dt..Rows != null && dt.Rows.Count != 0*/)
            {
                string strReportName = "CustmorReport_AR";
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
        private async Task<DataTable> GetCustmorData()
        {
            var sqlQuery = String.Format("SELECT id,location_cus,cusmor_name,contact_number,email FROM Custmor");

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
    }
}
