using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CoachingManagementSystem.Controllers
{
    public class SubjectUnitController : Controller
    {
        //
        // GET: /SujectUnit/
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /SujectUnit/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /SujectUnit/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /SujectUnit/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /SujectUnit/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /SujectUnit/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /SujectUnit/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /SujectUnit/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
