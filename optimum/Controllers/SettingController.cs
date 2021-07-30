using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CoachingManagementSystem.Controllers
{
    public class SettingController : Controller
    {
        // GET: Setting
        public ActionResult CreateSession()
        {
            return View();
        }
    }
}