using WaterCompanySystem.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WaterCompanySystem.Controllers
{
    public class ReportsController : Controller
    {
        // GET: Reports
        public ActionResult Index()
        {
            return View();
        }
        public DataTable GetDataTable()
        {
            // Define the DataTable
            DataTable dataTable = new DataTable("TransactionData");
            dataTable.Columns.Add("suplier_type_id", typeof(int));
            dataTable.Columns.Add("quantity", typeof(int));

            using (var context = new WaterComponySystemEntities())
            {
                // Example using LINQ to fill the DataTable
                var query = context.Warehouses
                    .Where(x => x.quantity > 0) // Example filter
                    .Select(x => new { x.suplier_type_id, x.quantity })
                    .ToList();

                // Fill the DataTable with data from the query
                foreach (var item in query)
                {
                    dataTable.Rows.Add(item.suplier_type_id, item.quantity);
                }
            }

            return dataTable;
        }

    }
}