using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WaterCompanySystem.ViewModel
{
    public class LoginVM
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string SelectedLanguage { get; set; } // Holds the selected language value
        public IEnumerable<SelectListItem> Languages { get; set; } // List of languages for the dropdown
    }
    }