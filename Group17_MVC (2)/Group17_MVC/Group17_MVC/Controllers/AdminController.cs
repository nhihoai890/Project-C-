using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Group17_MVC.Controllers
{
    public class AdminController : Controller
    {
        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Logout()
        {
            
            
            return RedirectToAction("Index", "Home");
        }

    }
}