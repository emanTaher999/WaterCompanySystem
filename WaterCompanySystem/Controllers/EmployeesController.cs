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
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Newtonsoft.Json.Linq;
using WaterCompanySystem.Models;
using WaterCompanySystem.Reports;


namespace WaterCompanySystem.Controllers
{
    public class EmployeesController : Controller
    {
        private WaterComponySystemEntities db = new WaterComponySystemEntities();

        // GET: Employees
        public ActionResult Index()
        {
            var employees = db.Employees.Include(e => e.Position).Include(e => e.UserTable);
            return View(employees.ToList());
        }
            public ActionResult IndexReport()
        {
            var employees = db.Employees.Include(e => e.Position).Include(e => e.UserTable);
            return View(employees.ToList());
        }

        // GET: Employees/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // GET: Employees/Create
        public ActionResult Create()
        {
            ViewBag.position_id = new SelectList(db.Positions, "id", "Pos_type");
            ViewBag.userid = new SelectList(db.UserTables, "id", "username");
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,emp_name,position_id,salary,hire_date,vacation_days,alwance,userid")] Employee employee)
        {
           try
            {
                if (employee.alwance == null)
                    employee.alwance = 0;
                db.Employees.Add(employee);
                db.SaveChanges();
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

            ViewBag.position_id = new SelectList(db.Positions, "id", "Pos_type", employee.position_id);
            ViewBag.userid = new SelectList(db.UserTables, "id", "username", employee.userid);
            return View(employee);
        }

        // GET: Employees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            ViewBag.position_id = new SelectList(db.Positions, "id", "Pos_type", employee.position_id);
            ViewBag.userid = new SelectList(db.UserTables, "id", "username", employee.userid);
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,emp_name,position_id,salary,hire_date,vacation_days,alwance,userid")] Employee employee)
        {
            try
            {
                db.Entry(employee).State = EntityState.Modified;
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
            ViewBag.position_id = new SelectList(db.Positions, "id", "Pos_type", employee.position_id);
            ViewBag.userid = new SelectList(db.UserTables, "id", "username", employee.userid);
            return View(employee);
        }

        // GET: Employees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            ViewBag.position_id = new SelectList(db.Positions, "id", "Pos_type", employee.position_id);
            ViewBag.userid = new SelectList(db.UserTables, "id", "username", employee.userid);
            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Employee employee = db.Employees.Find(id);
            db.Employees.Remove(employee);
            db.SaveChanges();
            TempData["AlertMessage"] = "deleted";
         
            ViewBag.position_id = new SelectList(db.Positions, "id", "Pos_type", employee.position_id);
            ViewBag.userid = new SelectList(db.UserTables, "id", "username", employee.userid);
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

        /////////////////////////////////////////////////////////////////////////////////////////////
        public async Task<ActionResult> EmpData()
        {
            return View();
        }
        public JsonResult GetEmpDataReport()
        {
            var dt = GetEmpData();

            if (dt != null /*&& dt..Rows != null && dt.Rows.Count != 0*/)
            {
                string strReportName = "EmployeeRpt";
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
        private async Task<DataTable> GetEmpData()
        {
            var sqlQuery = String.Format("select e.id,emp_name,Pos_type,salary,username,alwance,hire_date,vacation_days from Employee e join Position p on e.position_id=p.id join UserTable u on e.userid=u.id");
        
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

                Cmd.Connection =SQLConn ;
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
