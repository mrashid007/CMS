using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CoachingEntity.Model;
using CoachingManagementSystem.CoachingEntity.CommonModel;
using CoachingManagementSystem.Models;
using CoachingManagementSystem.Models.CommonModel;
using CoachingManager.Manager;

namespace CoachingManagementSystem.Controllers
{
    public class InstituteController : Controller
    {
        InstituteManager _instituteManager=new InstituteManager();
        //
        // GET: /Institute/
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Institute/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Institute/Create
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Save(string insCode, string insName)
        {
            Result result = new Result();
            Institute institute = new Institute
            {
                Code = insCode,
                Name = insName
            };

            result = _instituteManager.SaveInstitute(institute);

            result.Message = "Successfully Saved";
            return Json(result);
        }
        //
        // POST: /Institute/Create
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
        // GET: /Institute/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Institute/Edit/5
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
        // GET: /Institute/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Institute/Delete/5
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

        public JsonResult GetInstitutes()
        {
            List<Institute> list = new List<Institute>();
            list = _instituteManager.GetInstitutes();
            var insList = list.Select(x => new { Name = x.Name, Id = x.Id, Edit = "<div class = 'btnEdit' id =" + x.Id.ToString() + "'  title = 'Edit' onclick='EditBatch(" + x.Id + ")'></div>", }).ToList();
            return Json(insList.ToArray(), JsonRequestBehavior.AllowGet);
        }
    }
}
