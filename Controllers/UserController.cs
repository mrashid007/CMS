using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CoachingEntity.Model;
using CoachingManager.Manager;
using Microsoft.AspNet.Identity;
using CoachingManagementSystem.CoachingEntity.CommonModel;
using CoachingManagementSystem.Helper;

namespace CoachingManagementSystem.Controllers
{
    public class UserController : Controller
    {
        SecurityUserManager _securityUserManager=new SecurityUserManager();
        EmployeeManager _employeeManager = new EmployeeManager();
        //
        // GET: /User/
        public ActionResult Index()
        {
            return View();
        }
        //
        // GET: /User/
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(string employeeName,string contactNo,string email, string userName, string password, long roleId, long designationId)
        {
            Dictionary<int, CheckSessionData> IDictionary = CheckSessionData.GetSessionValues();
            long companyId = (long)IDictionary[1].Id;
            long locationId = (long)IDictionary[2].Id;
            long userId = (long)IDictionary[3].Id;
            //string user = (string)IDictionary[3].Name;
            Result result=new Result();
            try
            {
                Employee employee = new Employee
                {
                    Name=employeeName,
                    ContactNo=contactNo,
                    DateOfEntry=DateTime.Now,
                    DesignationId=designationId,
                    Email=email,
                    UserId=userId                    
                };

                result = _employeeManager.SaveEmployee(employee);
                result.OptionNo = employee.EmployeeNo;

                if(result.IsSuccess==true)
                {
                    SecurityUser securityUser = new SecurityUser
                    {
                        UserName = userName,
                        Name = userName,
                        Password = password,
                        RoleId = roleId,
                        CompanyId = companyId,
                        LocationId = locationId
                    };
                    Result result1 = new Result();
                    result1 = _securityUserManager.SavesecurityUsers(securityUser);

                    if(result1.IsSuccess)
                    {
                        result.Message = result1.Message;                        
                    }
                }              

               // return RedirectToAction("Index");
            }
            catch
            {
                //return View();
            }
            return Json(result,JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetSecurityUser()
        {
            List<SecurityUser> list = new List<SecurityUser>();
            list = _securityUserManager.GetSecurityUsers();
            var userList = list.Select(x => new { Name = x.Name, Id = x.UserId,Role=x.UserRole.RoleName,Edit = "<div class = 'btnEdit' id =" + x.UserId.ToString() + "'  title = 'Edit' onclick='EditBatch(" + x.UserId + ")'></div>", }).ToList();
            return Json(userList.ToArray(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadUserRole()
        {
            List<UserRole> userRoles = new List<UserRole>();
            userRoles = _securityUserManager.LoadUserRole();
            var examlist = userRoles.Select(x => new { val = x.RoleId, text = x.RoleName }).ToList();
            return Json(examlist.ToArray(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult ChangePassword()
        {
            Dictionary<int, CheckSessionData> IDictionary = CheckSessionData.GetSessionValues();
            long companyId = (long)IDictionary[1].Id;
            long locationId = (long)IDictionary[2].Id;
            long userId = (long)IDictionary[3].Id;
            string user = (string)IDictionary[3].Name;

            SecurityUser securityUser = _securityUserManager.SelectSecurityUser(new SecurityUser { UserName=user,UserId=userId});
            List<UserRole> userRoleList = _securityUserManager.LoadUserRole();
            ViewBag.RoleId = new SelectList(userRoleList, "RoleId", "RoleName", securityUser.RoleId);

            //SecurityUser securityUser = new SecurityUser
            //{
            //    UserName = user,
            //    Password=
            //};

            return View(securityUser);
        }

        [HttpPost]
        public ActionResult ChangePassword(SecurityUser user)
        {
            Dictionary<int, CheckSessionData> IDictionary = CheckSessionData.GetSessionValues();
            long companyId = (long)IDictionary[1].Id;
            long locationId = (long)IDictionary[2].Id;
            long userId = (long)IDictionary[3].Id;
            string userName = (string)IDictionary[3].Name;
            Result result = new Result();
            //SecurityUser securityUser = _securityUserManager.SelectSecurityUser(new SecurityUser { UserName = userName, UserId = userId });

            //securityUser.UserName = user.UserName;
            //securityUser.Password = user.Password;
            //securityUser.RoleId = user.RoleId;

            user.UserId = userId;

            result = _securityUserManager.UpdateSecurityUsers(user);

            //return View(securityUser);
            return Json(result,JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadDesignation()
        {
            List<Designation> designations = new List<Designation>();
            designations = _securityUserManager.LoadDesignations();
            var designationList = designations.Select(x => new { val = x.DesignationId, text = x.Name }).ToList();
            return Json(designationList.ToArray(), JsonRequestBehavior.AllowGet);
        }
	}
}