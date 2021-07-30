using CoachingEntity.Model;
using CoachingManager.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CoachingManagementSystem.Controllers
{
    public class CoachingSessionController : Controller
    {
        CommonManager commonManager = new CommonManager();

        // GET: CoachingSession
        public ActionResult Index()
        {
            return View();
        }
        // GET: CoachingSession
        public ActionResult CreateSession()
        {
            return View();
        }
        public JsonResult GetSessions()
        {
            List<CoachingSession> list = new List<CoachingSession>();
            list = commonManager.GetSessions();
            var sessionList = list.Select(x => new { Name = x.Session, Id = x.SessionId, Edit = "<div class = 'btnEdit' id =" + x.SessionId.ToString() + "'  title = 'Edit' onclick='EditSession(" + x.SessionId + ")'></div>", }).ToList();
            return Json(sessionList.ToArray(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult LoadCoachingSessions()
        {
            List<CoachingSession> list = new List<CoachingSession>();
            list = commonManager.GetSessions();
            var sessionList = list.Select(x => new { Name = x.Session, Id = x.SessionId }).OrderBy(x => x.Id).ToList();
            return Json(sessionList.ToArray(), JsonRequestBehavior.AllowGet);
        }
    }
}