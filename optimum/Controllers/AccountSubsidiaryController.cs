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
    public class AccountSubsidiaryController : Controller
    {
        AccountsManager accountsManager=new AccountsManager();
        CommonManager _commonManager=new CommonManager();
        //
        // GET: /AccountSubsidiary/
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /AccountSubsidiary/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /AccountSubsidiary/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /AccountSubsidiary/Create
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
        // GET: /AccountSubsidiary/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /AccountSubsidiary/Edit/5
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
        // GET: /AccountSubsidiary/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /AccountSubsidiary/Delete/5
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
        // POST: /AccountSubSidiary/Save
        [HttpPost]
        public ActionResult Save(AccountSubsSidiaryVM accountSubsidiaryVm)
        {
            Result result = new Result();
            Dictionary<int, CheckSessionData> IDictionary = CheckSessionData.GetSessionValues();
            long companyId = (long)IDictionary[1].Id;
            long locationId = (long)IDictionary[2].Id;
            long userId = (long)IDictionary[3].Id;
            AccountSubsidiary accountGroup = new AccountSubsidiary
            {
                Name = accountSubsidiaryVm.Name,
                Code = accountSubsidiaryVm.Code,
                AccGroupId = accountSubsidiaryVm.AccountGroupId,
                AccSubGroupId =accountSubsidiaryVm.AccountSubGroupId,
                AccControlId = accountSubsidiaryVm.AccountControlId,
                DateOfEntry = DateTime.Now,
                EntryBy = userId,
            };
            result = accountsManager.SaveAccountSubSidiary(accountGroup);

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
                List<AccountSubsidiary> _FinalList = new List<AccountSubsidiary>();
                List<AccountSubsidiary> result = accountsManager.GetAccountSubsidiaries();//.SelectBuyerForIndex(new Party { CompanyId = companyId });

                List<AccountSubsidiary> _CopyList = new List<AccountSubsidiary>();
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
                                        AccountSubsidiaryId = obj.AccountSubsidiaryId,
                                        Name = obj.Name,
                                        Code = obj.Code,
                                        GroupName=obj.AccountGroup.Name,
                                        SubGroupName=obj.AccountSubGroup.Name,
                                        ControlName=obj.AccountControl.Name,
                                        Edit = obj.AccountSubsidiaryId == 0 ? "" : "<a class = 'btnEdit' href=" + Url.Action("Edit/" + obj.AccountSubsidiaryId.ToString()) + "</a>",
                                        Delete = obj.AccountSubsidiaryId == 0 ? "" : "<div class = 'btnDelete' id = " + obj.AccountSubsidiaryId.ToString() + " onclick='DeleteData(" + obj.AccountSubsidiaryId.ToString() + "," + "&apos;" + "AccountSubsidiary" + "&apos;" + "," + "&apos;" + ")'></div>"
                                    }
                         });
                return value;
            }
            else
                return "Session Out";
        }
    }
}
