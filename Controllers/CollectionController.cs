using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CoachingManagementSystem.Controllers
{
    public class CollectionController : Controller
    {
        //
        // GET: /Collection/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {
            return View();
        }
	}
}