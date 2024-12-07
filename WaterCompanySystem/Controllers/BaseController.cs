using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WaterCompanySystem.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Set layout from session
            if (Session["Layout"] != null)
            {
                ViewBag.Layout = Session["Layout"].ToString();
            }
            else
            {
                ViewBag.Layout = "~/Views/Shared/_LayoutENG.cshtml"; // Default layout
            }

            // Set language from session
            if (Session["language"] != null)
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(Session["language"].ToString());
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(Session["language"].ToString());
            }

            base.OnActionExecuting(filterContext);
        }

        // GET: Base
        public ActionResult Index()
        {
            return View();
        }

    }
}