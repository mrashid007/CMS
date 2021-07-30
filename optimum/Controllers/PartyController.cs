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
    public class PartyController : Controller
    {
        //
        // GET: /Party/
        PartyManager _partyManager = new PartyManager();
        AccountsManager accountsManager=new AccountsManager();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }
        public string LoadData(int? page, int? rows, string sort, string order, string searchName)
        {
            Dictionary<int, CheckSessionData> IDictionary = CheckSessionData.GetSessionValues();
            long companyId = (long)IDictionary[1].Id;
            if (companyId != 0)
            {
                int pageSize = (rows ?? 10);
                int pageIndex = ((page ?? 0));
                List<Party> _FinalList = new List<Party>();
                List<Party> partyList = _partyManager.SelectParty(new Party());

                List<Party> _CopyList = new List<Party>();
                _CopyList = partyList;
                if (!String.IsNullOrEmpty(searchName))
                    _FinalList = partyList.Where(x => x.Name.ToLower().Contains(searchName.ToLower()) || x.Code.ToLower().Contains(searchName.ToLower())).ToList();
                else
                    _FinalList = partyList;
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
                                        PartyId = obj.PartyId,
                                        Name = obj.Name,
                                        Code = obj.Code,
                                        Edit = obj.PartyId == 0 ? "" : "<a class = 'btnEdit' href=" + Url.Action("Edit/" + obj.PartyId.ToString()) + "</a>",
                                        Delete = obj.PartyId == 0 ? "" : "<div class = 'btnDelete' id = " + obj.PartyId.ToString() + " onclick='DeleteData(" + obj.PartyId.ToString() + "," + "&apos;" + "AccountHead" + "&apos;" + "," + "&apos;" + ")'></div>"
                                    }
                         });
                return value;
            }
            else
                return "Session Out";
        }

        public JsonResult Save(PartyVM partyinfo)
        {
            Dictionary<int, CheckSessionData> IDictionary = CheckSessionData.GetSessionValues();
            long companyId = (long)IDictionary[1].Id;
            long locationId = (long)IDictionary[2].Id;
            long userId = (long)IDictionary[3].Id;

            Result result=new Result();
            try
            {
                Party party = new Party
                {
                    PartyId = partyinfo.PartyId,
                    Code = partyinfo.Code,
                    Name = partyinfo.Name,
                    FatherName = partyinfo.FatherName,
                    MotherName = partyinfo.MotherName,
                    ContactNo = partyinfo.ContactNo,
                    ParmanentAddress = partyinfo.ParmanentAddress,
                    PresentAddress = partyinfo.PresentAddress,
                    DeptId = partyinfo.DeptId,
                    Email = partyinfo.Email,
                    NId = partyinfo.NId
                };
                result=_partyManager.InsertOrUpdate(party);

                if (result.IsSuccess)
                {
                    AccountSubsidiary accountSubsidiaryReceivable = new AccountSubsidiary();
                    accountSubsidiaryReceivable = accountsManager.GetAccountSubsidiaries().Where(x => x.Name.ToLower().Contains("receivable")).FirstOrDefault();
                    AccountSubsidiary accountSubsidiaryPayable = new AccountSubsidiary();
                    accountSubsidiaryPayable = accountsManager.GetAccountSubsidiaries().Where(x => x.Name.ToLower().Contains("payable")).FirstOrDefault();
                    string partyCode = result.OptionNo;

                    if (partyinfo.AccountType == 0 && accountSubsidiaryReceivable!=null)
                        {
                            AccountHead accountHead = new AccountHead
                            {
                                Name = partyinfo.Name + "(Borrower)",
                                Code = partyCode,
                                AccGroupId = accountSubsidiaryReceivable.AccGroupId,
                                AccSubGroupId = accountSubsidiaryReceivable.AccSubGroupId,
                                AccControlId = accountSubsidiaryReceivable.AccControlId,
                                AccSubsidiaryId = accountSubsidiaryReceivable.AccountSubsidiaryId,
                                OpeningBalance = 0,
                                CompanyId = companyId,
                                LocationId = locationId,
                                DateOfEntry = DateTime.Now,
                                EntryBy = userId,
                            };
                            result = accountsManager.SaveAccountHead(accountHead);
                        }
                        else if (partyinfo.AccountType == 1 && accountSubsidiaryPayable!=null)
                        {
                            AccountHead accountHead = new AccountHead
                            {
                                Name = partyinfo.Name + "(Donner)",
                                Code = partyCode,
                                AccGroupId = accountSubsidiaryPayable.AccGroupId,
                                AccSubGroupId = accountSubsidiaryPayable.AccSubGroupId,
                                AccControlId = accountSubsidiaryPayable.AccControlId,
                                AccSubsidiaryId = accountSubsidiaryPayable.AccountSubsidiaryId,
                                OpeningBalance = 0,
                                CompanyId = companyId,
                                LocationId = locationId,
                                DateOfEntry = DateTime.Now,
                                EntryBy = userId,
                            };
                            result = accountsManager.SaveAccountHead(accountHead); 
                        }
                    else if (partyinfo.AccountType == 2 && accountSubsidiaryReceivable != null && accountSubsidiaryPayable != null)
                        {
                            AccountHead accountHead = new AccountHead
                            {
                                Name = partyinfo.Name + "(Borrower)",
                                Code = partyCode,
                                AccGroupId = accountSubsidiaryReceivable.AccGroupId,
                                AccSubGroupId = accountSubsidiaryReceivable.AccSubGroupId,
                                AccControlId = accountSubsidiaryReceivable.AccControlId,
                                AccSubsidiaryId = accountSubsidiaryReceivable.AccountSubsidiaryId,
                                OpeningBalance = 0,
                                CompanyId = companyId,
                                LocationId = locationId,
                                DateOfEntry = DateTime.Now,
                                EntryBy = userId,
                            };
                            result = accountsManager.SaveAccountHead(accountHead);
                            AccountHead accountHeadCr = new AccountHead
                            {
                                Name = partyinfo.Name + "(Donner)",
                                Code = partyCode,
                                AccGroupId = accountSubsidiaryPayable.AccGroupId,
                                AccSubGroupId = accountSubsidiaryPayable.AccSubGroupId,
                                AccControlId = accountSubsidiaryPayable.AccControlId,
                                AccSubsidiaryId = accountSubsidiaryPayable.AccountSubsidiaryId,
                                OpeningBalance = 0,
                                CompanyId = companyId,
                                LocationId = locationId,
                                DateOfEntry = DateTime.Now,
                                EntryBy = userId,
                            };
                            result = accountsManager.SaveAccountHead(accountHeadCr);
                        }
                }
            }
            catch (Exception e)
            {
                result.IsSuccess = false;
                result.Message =e.Message;
            }
            
            return Json(result,JsonRequestBehavior.AllowGet);
        }
	}
}