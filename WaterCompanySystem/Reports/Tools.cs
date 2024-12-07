using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaterCompanySystem.Reports
{
    public class Tools
    {
        public string getProgectPath()
        {
            string path = HttpContext.Current.Server.MapPath("~/Reports/").ToString();
            return path;
        }
    }
}