using WaterCompanySystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using WaterCompanySystem.ViewModel;

namespace WaterCompanySystem.Controllers
{
    public class HomeController : Controller
    {
        private WaterComponySystemEntities db = new WaterComponySystemEntities();
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {

            // Create a new instance of the view model
            var viewModel = new LoginVM
            {
                // Populate the dropdown list with language options
                Languages = new List<SelectListItem>
        {
            new SelectListItem { Text = "Arabic", Value = "Ar" },
            new SelectListItem { Text = "English", Value = "en-US" }
            
        }  ,SelectedLanguage="Ar"   };

            // Pass the populated view model to the view
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult Login(LoginVM model)
        {
          if (ModelState.IsValid)
            {
                string lang="";
                // Example: Check user credentials (replace with your actual logic)
                var findUser = db.UserTables
                                 .Where(u => u.username == model.UserName && u.password == model.Password)
                                 .FirstOrDefault();

                if (findUser != null)
                {
                    // Set session variables for user and language preferences
                    Session["UserName"] = findUser.username;

                    // Set language session based on selection
                    if (model.SelectedLanguage == "Ar")
                    {
                        Session["language"] = "ar-SA";
                        lang = "ar-SA";
                    }
                    else
                    {
                        Session["language"] = "en-US";
                        lang = "en-US";
                    }
                    HttpCookie cultureCookie = new HttpCookie("culture",lang );
                    cultureCookie.Expires = DateTime.Now.AddYears(1); // Optional: set cookie expiry
                    Response.Cookies.Add(cultureCookie);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["msg"] = "Incorrect Username or Password .......!!";
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }
            model.Languages = new List<SelectListItem>
    {
        new SelectListItem { Text = "Arabic", Value = "Ar" },
        new SelectListItem { Text = "English", Value = "En" }
    };
            TempData.Keep("msg");
            return Redirect(Request.UrlReferrer.ToString());

           // return View(model);  
    }
        public ActionResult Logout()
        {
            Session["UserName"] = null;
            return RedirectToAction("Login");
        }
    }
}