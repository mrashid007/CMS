using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CoachingEntity;
using CoachingEntity.Model;
using CoachingManagementSystem.CoachingEntity.CommonModel;
using CoachingManagementSystem.Models;
using CoachingManagementSystem.Models.CommonModel;
using CoachingManager.Manager;

namespace CoachingManagementSystem.Controllers
{
    public class ExaminationController : Controller
    {
        ExaminationManager manager=new ExaminationManager();
        //
        // GET: /Examination/
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Examination/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Examination/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Examination/Create
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
        [HttpPost]
        public ActionResult Save(string examCode, string examName, int examTypeId)
        {
            Result result = new Result();
            Examination examination= new Examination
            {
                Code = examCode,
                Name = examName,
                ExamTypeId = examTypeId
            };

            result = manager.SaveExamination(examination);
            if(result.IsSuccess==true)
                result.Message = "Successfully Saved";

            return Json(result);
        }

        //
        // GET: /Examination/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Examination/Edit/5
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
        // GET: /Examination/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Examination/Delete/5
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
        public JsonResult GetExams()
        {
            List<Examination> list = new List<Examination>();
            list = manager.GetExamination();
            var examList = list.Select(x => new { Name = x.Name, Id = x.ExamId, Edit = "<div class = 'btnEdit' id =" + x.ExamId.ToString() + "'  title = 'Edit' onclick='EditBatch(" + x.ExamId + ")'></div>", }).ToList();
            return Json(examList.ToArray(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadExamType()
        {
            List<ExamType> examTypes = new List<ExamType>();
            examTypes = manager.GetExamType();
            var examlist = examTypes.Select(x => new { val = x.Id, text = x.Name }).ToList();
            return Json(examlist.ToArray(), JsonRequestBehavior.AllowGet);
        }
    }
}
