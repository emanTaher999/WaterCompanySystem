using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace WaterCompanySystem
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
            string cultureCode = "ar-SA"; // Default culture

            // You can get culture from a cookie or user preference
            HttpCookie cultureCookie = Request.Cookies["culture"];
            if (cultureCookie != null)
            {
                cultureCode = cultureCookie.Value;
            }

            System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo(cultureCode);

            // Set the culture and UI culture
            System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            System.Threading.Thread.CurrentThread.CurrentUICulture = culture;
        }
    }
}
