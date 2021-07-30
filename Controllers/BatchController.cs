using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CoachingEntity.Model;
using CoachingManagementSystem.CoachingEntity.CommonModel;
using CoachingManager;
using CoachingManagementSystem.Models;
using CoachingManagementSystem.Models.CommonModel;
using CoachingManager.Manager;

namespace CoachingManagementSystem.Controllers
{
    public class BatchController : Controller
    {
        BatchManager batchManager = new BatchManager();
        CommonManager commonManager = new CommonManager();
        //
        // GET: /Batch/
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Batch/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Batch/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Save(string batchCode,string batchName)
        {
            Result result=new Result();
            List<CoachingSession> coachingSes = new List<CoachingSession>();
            coachingSes = commonManager.GetActiveSessions().ToList();

            Batch batch = new Batch
            {
                Code = batchCode,
                Name = batchName,
                SessionId= coachingSes[0].SessionId
            };
            result = batchManager.SaveBatch(batch);

            result.Message = "Successfully Saved";
            return Json(result);
        }

        //
        // POST: /Batch/Create
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
        // GET: /Batch/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Batch/Edit/5
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
        // GET: /Batch/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Batch/Delete/5
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

        public JsonResult GetBatches()
        {
            List<CoachingSession> coachingSes = new List<CoachingSession>();
            coachingSes = commonManager.GetActiveSessions().ToList();

            List<Batch>list=new List<Batch>();
            list = batchManager.GetBatches().Where(x=>x.SessionId== coachingSes[0].SessionId).ToList();
            var batchList = list.Select(x => new { Name = x.Name, Id = x.Id, Edit = "<div class = 'btnEdit' id =" + x.Id.ToString() + "'  title = 'Edit' onclick='EditBatch(" + x.Id + ")'></div>", }).ToList();
            return Json(batchList.ToArray(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult SearchBatch(int sessionId,string batchCode,string batchName)
        {
            List<Batch> list = new List<Batch>();
            list = batchManager.GetBatches();
            if (sessionId != 0)
                list = list.Where(x => x.SessionId==sessionId).ToList();
            else
            {
                List<CoachingSession> coachingSes = new List<CoachingSession>();
                coachingSes = commonManager.GetActiveSessions().ToList();
                list = list.Where(c=>c.SessionId==coachingSes[0].SessionId).ToList();
            }
            if (batchCode !="")
                list = list.Where(x => x.Code.ToLower() == batchCode.ToLower()).ToList();
            if (batchName !="")
                list = list.Where(x => x.Name.ToLower() == batchName.ToLower()).ToList();

            var batchList = list.Select(x => new { Name = x.Name, Id = x.Id, Edit = "<div class = 'btnEdit' id =" + x.Id.ToString() + "'  title = 'Edit' onclick='EditBatch(" + x.Id + ")'></div>", }).OrderBy(x => x.Id).ToList();
            return Json(batchList.ToArray(), JsonRequestBehavior.AllowGet);
        }
    }
}
