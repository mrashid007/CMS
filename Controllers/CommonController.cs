using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CoachingEntity.Model;
using CoachingManagementSystem.Helper;
using CoachingManagementSystem.Models;
using CoachingManager.Manager;

namespace CoachingManagementSystem.Controllers
{
    public class CommonController : Controller
    {
        StudentManager _studentManager = new StudentManager();
        ExaminationManager examinationManager = new ExaminationManager();
        InstituteManager instituteManager = new InstituteManager();
        BatchManager batchManager = new BatchManager();
        CommonManager commonManager = new CommonManager();
        AccountsManager accountsManager=new AccountsManager();
        //
        // GET: /Common/
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult LoadExam()
        {
            List<Examination> examinations = new List<Examination>();
            examinations = examinationManager.GetExamination();
            var examlist = examinations.Select(x => new { val = x.ExamId, text = x.Name }).ToList();
            return Json(examlist.ToArray(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadYear()
        {
            List<Year> years = new List<Year>();
            years = commonManager.GetYears();
            var examlist = years.Select(x => new { val = x.Id, text = x.Name }).ToList();
            return Json(examlist.ToArray(), JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult LoadInstitute()
        {
            List<Institute> institutes = new List<Institute>();
            institutes = instituteManager.GetInstitutes();
            var insList = institutes.Select(x => new { val = x.Id, text = x.Name }).ToList();
            return Json(insList.ToArray(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadBoard()
        {
            List<Board> boards = new List<Board>();
            boards = commonManager.LoadBoards();
            var boardList = boards.Select(x => new { val = x.Id, text = x.Name }).ToList();
            return Json(boardList.ToArray(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadDepartment()
        {
            List<Department> departments = new List<Department>();
            departments = commonManager.LoadDepartments();
            var departmentList = departments.Select(x => new { val = x.Id, text = x.Name }).ToList();
            return Json(departmentList.ToArray(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadBatch()
        {
            List<CoachingSession> coachingSes = new List<CoachingSession>();
            coachingSes = commonManager.GetActiveSessions().ToList();

            List<Batch> batches = new List<Batch>();
            batches = batchManager.LoadBatches().Where(v=>v.SessionId==coachingSes[0].SessionId).ToList();
            var batchList = batches.Select(x => new { val = x.Id, text = x.Name }).ToList();
            return Json(batchList.ToArray(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadAccountGroup()
        {
            List<AccountGroup> accountGroups = new List<AccountGroup>();
            accountGroups = accountsManager.GetAccountGroups();
            var accountlist = accountGroups.Select(x => new { val = x.AccountGroupId, text = x.Name }).ToList();
            return Json(accountlist.ToArray(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadAccountSubGroup(long accountGroupId = 0)
        {
            List<AccountSubGroup> accountSubGroups = new List<AccountSubGroup>();
            if (accountGroupId>0)
                accountSubGroups = accountsManager.GetAccountSubGroups().Where(x => x.AccGroupId == accountGroupId).ToList();
            else
                accountSubGroups = accountsManager.GetAccountSubGroups();
            var accountSubList = accountSubGroups.Select(x => new { val = x.AccountSubGroupId, text = x.Name }).ToList();
            return Json(accountSubList.ToArray(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadAccountControl(long accountGroupId = 0, long accSubId=0)
        {
            List<AccountControl> accountControls = new List<AccountControl>();
            accountControls = accountsManager.GetAccountControls();
            if (accountGroupId>0)
                accountControls=accountControls.Where(x => x.AccGroupId == accountGroupId).ToList();
            if (accSubId > 0)
                accountControls = accountControls.Where(x => x.AccSubGroupId == accSubId).ToList();
            var accountControlList = accountControls.Select(x => new { val = x.AccountControlId, text = x.Name }).ToList();
            return Json(accountControlList.ToArray(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadAccountSubsidiary(long accountGroupId = 0, long accSubId = 0, long accCtrlId = 0)
        {
            List<AccountSubsidiary> accountSubsidiaries = new List<AccountSubsidiary>();
            accountSubsidiaries = accountsManager.GetAccountSubsidiaries();
            if (accountGroupId > 0)
                accountSubsidiaries = accountSubsidiaries.Where(x => x.AccGroupId == accountGroupId).ToList();
            if (accSubId > 0)
                accountSubsidiaries = accountSubsidiaries.Where(x => x.AccSubGroupId == accSubId).ToList();
            if (accCtrlId > 0)
                accountSubsidiaries = accountSubsidiaries.Where(x => x.AccControlId == accCtrlId).ToList();
            var accountSubList = accountSubsidiaries.Select(x => new { val = x.AccountSubsidiaryId, text = x.Name }).ToList();
            return Json(accountSubList.ToArray(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadAccountHead(long accgrpId = 0, long accSubGrpId = 0, long accCtrlId = 0,long accSubId=0)
        {
            List<AccountHead> accountHeadsList = new List<AccountHead>();
            accountHeadsList = accountsManager.GetAccountHeads();
            if (accgrpId > 0)
                accountHeadsList = accountHeadsList.Where(x => x.AccGroupId == accgrpId).ToList();
            if (accSubGrpId > 0)
                accountHeadsList = accountHeadsList.Where(x => x.AccSubGroupId == accSubGrpId).ToList();
            if (accCtrlId > 0)
                accountHeadsList = accountHeadsList.Where(x => x.AccControlId == accCtrlId).ToList();
            if (accSubId > 0)
                accountHeadsList = accountHeadsList.Where(x => x.AccControlId == accCtrlId).ToList();
            var accountheadlist = accountHeadsList.Select(x => new { val = x.AccountHeadId, text = x.Name }).ToList();
            return Json(accountheadlist.ToArray(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetVat()
        {
            Dictionary<int, CheckSessionData> IDictionary = CheckSessionData.GetSessionValues();
            long companyId = (long)IDictionary[1].Id;
            long locationId = (long)IDictionary[2].Id;
            long userId = (long)IDictionary[3].Id;

            List<VatTax> vatTaxs = commonManager.GetVat();
            var vatlist = vatTaxs.Select(x => new { Vat = x.Vat, VatType = x.VatType }).ToList();
            return Json(vatlist.ToArray(), JsonRequestBehavior.AllowGet);
        }
	}
}