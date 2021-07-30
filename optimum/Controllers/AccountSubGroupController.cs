using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using CoachingEntity.Model;
using CoachingManagementSystem.CoachingEntity.CommonModel;
using CoachingManagementSystem.Helper;
using CoachingManagementSystem.Models.CommonModel;
using CoachingManager.Manager;

namespace CoachingManagementSystem.Controllers
{
    public class AccountSubGroupController : Controller
    {
        AccountsManager accountsManager=new AccountsManager();
        //
        // GET: /AccountSubGroup/
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /AccountSubGroup/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /AccountSubGroup/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /AccountSubGroup/Create
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
        // GET: /AccountSubGroup/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /AccountSubGroup/Edit/5
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
        // GET: /AccountSubGroup/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /AccountSubGroup/Delete/5
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
        //
        // POST: /AccountSubGroup/Save
        [HttpPost]
        public ActionResult Save(AccountSubGroupVM accountSubGroupVm)
        {
            Result result = new Result();
            Dictionary<int, CheckSessionData> IDictionary = CheckSessionData.GetSessionValues();
            long companyId = (long)IDictionary[1].Id;
            long locationId = (long)IDictionary[2].Id;
            long userId = (long)IDictionary[3].Id;
            AccountSubGroup accountGroup = new AccountSubGroup
            {
                Name = accountSubGroupVm.Name,
                Code = accountSubGroupVm.Code,
                AccGroupId = accountSubGroupVm.AccountGroupId,
                DateOfEntry = DateTime.Now,
                EntryBy = userId,
            };
            result = accountsManager.SaveSubGroup(accountGroup);

            return Json(result, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public string LoadData(int? page, int? rows, string sort, string order, string searchName)
        {
            Dictionary<int, CheckSessionData> IDictionary = CheckSessionData.GetSessionValues();
            long companyId = (long)IDictionary[1].Id;
            if (companyId != 0)
            {
                int pageSize = (rows ?? 10);
                int pageIndex = ((page ?? 0));
                List<AccountSubGroup> _FinalList = new List<AccountSubGroup>();
                List<AccountSubGroup> result = accountsManager.GetAccountSubGroups();//.SelectBuyerForIndex(new Party { CompanyId = companyId });

                List<AccountSubGroup> _CopyList = new List<AccountSubGroup>();
                _CopyList = result;
                if (!String.IsNullOrEmpty(searchName))
                    _FinalList = result.Where(x => x.Name.ToLower().Contains(searchName.ToLower()) || x.Code.ToLower().Contains(searchName.ToLower())).ToList();
                else
                    _FinalList = result;
                if (_FinalList.Count == 0)
                {
                    //_CopyList.RemoveAll(x => x.Name == null);
                    if (_CopyList.Count > 0)
                        _FinalList = _CopyList.Where(x => x.Name.ToLower().Contains(searchName.ToLower())).ToList();
                    if (_FinalList.Count == 0)
                    {
                        //_CopyList.RemoveAll(x => x.EmployeeName == null);
                        if (_CopyList.Count > 0)
                            _FinalList = _CopyList.Where(x => x.Name.ToLower().Contains(searchName.ToLower())).ToList();
                    }
                }
                if (!String.IsNullOrEmpty(sort) && sort.Equals("Name"))
                {
                    if (!String.IsNullOrEmpty(order) && order.Equals("asc"))
                        _FinalList = _FinalList.OrderBy(x => x.Name).ToList();
                    else if (!String.IsNullOrEmpty(order) && order.Equals("desc"))
                        _FinalList = _FinalList.OrderByDescending(x => x.Name).ToList();
                }

                else if (!String.IsNullOrEmpty(sort) && sort.Equals("Code"))
                {
                    if (!String.IsNullOrEmpty(order) && order.Equals("asc"))
                        _FinalList = _FinalList.OrderBy(x => x.Code).ToList();
                    else if (!String.IsNullOrEmpty(order) && order.Equals("desc"))
                        _FinalList = _FinalList.OrderByDescending(x => x.Code).ToList();
                }

                var totalData = 0;
                if (_FinalList.Count > 0)
                    totalData = int.Parse(_FinalList.Count.ToString());

                int SelectFrom = (int)((page - 1) * rows);
                int SelectTo = (int)(rows);

                ;
                int _GetRowsCount = 0;
                _FinalList = _FinalList.Skip(SelectFrom).Take(SelectTo).ToList();
                _GetRowsCount = _FinalList.Count();


                string value =
                         new JavaScriptSerializer().Serialize(new
                         {
                             total = totalData,
                             rows = from obj in _FinalList
                                    select new
                                    {
                                        AccountSubGroupId = obj.AccountSubGroupId,
                                        Name = obj.Name,
                                        Code = obj.Code,
                                        GroupName=obj.AccountGroup.Name,
                                        Edit = obj.AccountSubGroupId == 0 ? "" : "<a class = 'btnEdit' href=" + Url.Action("Edit/" + obj.AccountSubGroupId.ToString()) + "</a>",
                                        Delete = obj.AccountSubGroupId == 0 ? "" : "<div class = 'btnDelete' id = " + obj.AccountSubGroupId.ToString() + " onclick='DeleteData(" + obj.AccountSubGroupId.ToString() + "," + "&apos;" + "AccountSubGroup" + "&apos;" + "," + "&apos;" + ")'></div>"
                                    }
                         });
                return value;
            }
            else
                return "Session Out";
        }
    }
}
