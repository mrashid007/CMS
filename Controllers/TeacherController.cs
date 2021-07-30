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
    public class TeacherController : Controller
    {
        TeacherManager _teacherManager=new TeacherManager();
        CommonManager commonManager = new CommonManager();
        //
        // GET: /Teacher/
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Teacher/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Teacher/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Teacher/Create
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
        // GET: /Teacher/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Teacher/Edit/5
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
        // GET: /Teacher/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Teacher/Delete/5
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

        [HttpPost]
        public JsonResult Save(TeacherVM GeneralInfo, List<CertificatesVm> certificatesVms, HttpPostedFileBase profImage)
        {
            Result result = new Result();
            Guid id = Guid.NewGuid();
            Session["TeacherId"] = id.ToString();
            List<CoachingSession> coachingSes = new List<CoachingSession>();
            coachingSes = commonManager.GetActiveSessions().ToList();

            Teacher aTeacher = new Teacher
            {
                TeacherId = id,
                SessionId= coachingSes[0].SessionId,
                BloodGroup = GeneralInfo.BloodGroup,
                Name = GeneralInfo.TeacherName,
                FatherName = GeneralInfo.FatherName,
                MotherName = GeneralInfo.MotherName,
                ContactNo = GeneralInfo.TeacherMobile,
                Email = GeneralInfo.Email,
                DepartmentId = Convert.ToInt32(GeneralInfo.DepartmentId),
                PresentAddress = GeneralInfo.PresentAddress,
                ParmanentAddress = GeneralInfo.ParmanentAddress,
                Remarks = GeneralInfo.Remarks,
                ProfilePic = Server.MapPath("/Upload/Profile/Teacher/") + id.ToString() + ".jpeg"
            };

            result = _teacherManager.Save(aTeacher);
            if (result.IsSuccess)
            {
                List<Certificates> certificateses = new List<Certificates>();

                Certificates certificates;
                foreach (CertificatesVm certificatesVm in certificatesVms)
                {
                    certificates = new Certificates()
                    {
                        BoardId = certificatesVm.BoardId,
                        StudentId = id,
                        DepartmentId = certificatesVm.DeptId,
                        InstituteId = certificatesVm.InstId,
                        YearId = certificatesVm.YearId,
                        Result = certificatesVm.Result
                    };
                    certificateses.Add(certificates);
                }
               // result = commonManager.SaveCertificates(certificateses);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public void SaveProfileImage()
        {
            if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
            {
                var pic = System.Web.HttpContext.Current.Request.Files["MyImages"];
                if (pic.ContentLength > 0)
                {
                    Byte[] destination1 = new Byte[pic.ContentLength];
                    pic.InputStream.Position = 0;
                    pic.InputStream.Read(destination1, 0, pic.ContentLength);

                    Session["ProfilePic"] = destination1;


                    string _imgname = Session["TeacherId"].ToString();
                    var _comPath = Server.MapPath("/Upload/Profile/Teacher/") + _imgname + ".jpeg";

                    ViewBag.Msg = _comPath;

                    //Saving Image in Original Mode
                    pic.SaveAs(_comPath);

                    // //resizing image
                    //MemoryStream ms = new MemoryStream();
                    //WebImage img = new WebImage(_comPath);

                    //if (img.Width > 200)
                    //    img.Resize(200, 200);
                    //img.Save(_comPath);
                    //// end resize
                }
            }
        }
    }
}
