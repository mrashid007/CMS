using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using CoachingManagementSystem.CoachingEntity.CommonModel;
using CoachingManagementSystem.Models;
using CoachingManagementSystem.Models.CommonModel;
using CoachingManager.Manager;
using Microsoft.AspNet.Identity;
using SecurityUser = CoachingEntity.Model.SecurityUser;

namespace CoachingManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        SecurityUserManager userManager = new SecurityUserManager();
        private readonly Random _random = new Random();
        //
        // GET: /Account/
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Account/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Account/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Account/Create
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
        // GET: /Account/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Account/Edit/5
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
        // GET: /Account/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Account/Delete/5
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
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
             if (ModelState.IsValid)
             {
                 SecurityUser securityUser = userManager.GetSecurityUser(new SecurityUser { UserName = model.UserName, Password = model.Password });
                 
                 
                 if (securityUser != null)
                 {
                     System.Web.HttpContext.Current.Session["LoginCompany"] = securityUser.CompanyId;
                     System.Web.HttpContext.Current.Session["LoginLocation"] = securityUser.LocationId;
                     System.Web.HttpContext.Current.Session["LoginUser"] = securityUser.UserId;
                     System.Web.HttpContext.Current.Session["LoginUserName"] = securityUser.UserName;

                    string OTPStatus = ConfigurationManager.AppSettings["LoginOTP"].ToLower();
                    if(OTPStatus=="on")
                    {
                        securityUser.OTPCode = RandomPassword();
                        userManager.UpdateSecurityUsers(securityUser);
                        return RedirectToAction("OTPConfirmation", "Account", new { userId = securityUser.UserId.ToString(),msg= "OTP code already sent to registered mobile number." });
                    }

                    if (securityUser.UserId > 0)
                         return RedirectToAction("Home", "Home");
                     else
                     {
                         ModelState.AddModelError("LOGIN_ERROR", "Incorrect User Name or Password.");
                         return RedirectToAction("Login", "Account");
                     }
                 }
                 else
                 {
                     ModelState.AddModelError("LOGIN_ERROR", "Incorrect User Name or Password.");
                     return RedirectToAction("Login", "Account",1);
                 }
             }
             else
             {
                 return RedirectToAction("LogIn");
             }
        }
        public string RandomPassword()
        {
            var passwordBuilder = new StringBuilder();

            // 4-Letters lower case   
            passwordBuilder.Append(RandomString(4, true));

            // 4-Digits between 1000 and 9999  
            passwordBuilder.Append(RandomNumber(1000, 9999));

            // 2-Letters upper case  
            passwordBuilder.Append(RandomString(2));
            return passwordBuilder.ToString();
        }
        public int RandomNumber(int min, int max)
        {
            return _random.Next(min, max);
        }
        public string RandomString(int size, bool lowerCase = false)
        {
            var builder = new StringBuilder(size);

            // Unicode/ASCII Letters are divided into two blocks
            // (Letters 65–90 / 97–122):
            // The first group containing the uppercase letters and
            // the second group containing the lowercase.  

            // char is a single Unicode character  
            char offset = lowerCase ? 'a' : 'A';
            const int lettersOffset = 26; // A...Z or a..z: length=26  

            for (var i = 0; i < size; i++)
            {
                var @char = (char)_random.Next(offset, offset + lettersOffset);
                builder.Append(@char);
            }

            return lowerCase ? builder.ToString().ToLower() : builder.ToString();
        }
        public ActionResult OTPConfirmation(string userId,string msg)
        {
            ViewBag.UserId = userId;
            ViewBag.Msg = msg;
            return View();
        }
        public ActionResult OTPConfirmationSubmit(string userId,string otp)
        {
            Result result = userManager.CheckOTPCode(Convert.ToInt32(userId),otp);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Login(int value=0)
        {
            if (value == 1)
                ViewBag.Message = "User Name Or Password is incorrect";
            else
                ViewBag.Message = "";
            return View();
        }
        public ActionResult LogOff()
        {
            return RedirectToAction("LogIn");
        }
    }
}
