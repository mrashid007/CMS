using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CoachingEntity.Model;
using CoachingManagementSystem.CoachingEntity.CommonModel;
using CoachingManager.Manager;
using Microsoft.Ajax.Utilities;

namespace CoachingManagementSystem.Controllers
{
    public class DepartmentController : Controller
    {
        DepartmentManager _departmentManager=new DepartmentManager();
        //
        // GET: /Department/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {
            return View();
        }
        public ActionResult Edit()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Save(string Code, string Name)
        {
            Result result = new Result();
            Department department = new Department
            {
                Code = Code,
                Name = Name
            };

            result = _departmentManager.SaveDepartment(department);
            
            result.Message = "Successfully Saved";
            return Json(result);
        }

        public ActionResult GetDepartments()
        {
            List<Department> list = new List<Department>();
            list = _departmentManager.GetDepartments();
            var departmentList = list.Select(x => new { Name = x.Name, Id = x.Id, Edit = "<div class = 'btnEdit' id =" + x.Id.ToString() + "'  title = 'Edit' onclick='EditBatch(" + x.Id + ")'></div>", }).ToList();
            return Json(departmentList.ToArray(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetDepartmentsForDropdown()
        {
            List<Department> list = new List<Department>();
            list = _departmentManager.GetDepartments().DistinctBy(x=>x.Name).ToList();
            var departmentList = list.Select(x => new { Name = x.Name, Id = x.Id}).ToList();
            return Json(departmentList.ToArray(), JsonRequestBehavior.AllowGet);
        }
	}
}