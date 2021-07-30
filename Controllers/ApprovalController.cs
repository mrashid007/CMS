using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CoachingEntity.Model;
using CoachingManagementSystem.CoachingEntity.CommonModel;
using CoachingManagementSystem.Helper;
using CoachingManagementSystem.Models.CommonModel;
using CoachingManager.Manager;

namespace CoachingManagementSystem.Controllers
{
    public class ApprovalController : Controller
    {
        VoucherManager _voucherManager = new VoucherManager();
        //
        // GET: /Approval/
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetOptionList(string type, string fromDate, string todate)
        {
            List<OptionNos> list = new List<OptionNos>();

            switch (type)
            {
                case "V":
                    list = GetVoucherList(fromDate, todate);
                    break;
            }
            //var items = list.Select(x => new OptionNos { OptionId =, OptionNo = x.VoucherNo, Amount }).ToList();
            return Json(list.ToArray(), JsonRequestBehavior.AllowGet);

        }

        private List<OptionNos> GetVoucherList(string fromDate, string toDate)
        {
            List<Voucher> list = new List<Voucher>();
            list = _voucherManager.SelectUnApprovedVouchers().ToList();
            if (fromDate != "")
            {
                DateTime frmDate = Convert.ToDateTime(fromDate);
                list = list.Where(x => x.VoucherDate > frmDate).ToList();
            }
            if (toDate != "")
            {
                DateTime todate = Convert.ToDateTime(toDate).AddDays(1);
                list = list.Where(x => x.VoucherDate < todate).ToList();
            }

            return list.Select(x => new OptionNos { OptionId = x.VoucherId.ToString(), OptionNo = x.VoucherNo, Amount = x.VoucherDetails.Sum(c => c.Credit), Details = @"<div class = 'btnDetail' id = '" + x.VoucherId.ToString() + "' title = 'Detail' onclick=DetailView('" + x.VoucherId.ToString() + "','" + x.VoucherNo + @"')></div>" }).ToList();
        }

        public JsonResult GetVoucherDetails(string voucherId, string voucherno)
        {
           List<VoucherDetails> voucherDetails = new List<VoucherDetails>();
           voucherDetails = _voucherManager.SelectUnApprovedVoucherDetails(new VoucherDetails { VoucherId = new Guid(voucherId), Voucher = new Voucher { VoucherNo = voucherno } }).ToList();//.Where(c=>c.VoucherNo==voucherno).Select(x=>x.VoucherDetails.ToList()
            var item = voucherDetails.Select(x => new {Name = x.AccountHead.Name, Amount = x.Credit + x.Debit,Detail=x.Particulers}).ToList();
            return Json(item.ToArray(),JsonRequestBehavior.AllowGet);
        }

        public JsonResult Approval(List<OptionNos>list,string type )
        {
            Result result=new Result();

            switch (type)
            {
                case "V":
                    result = VoucherApproval(list);
                    break;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private Result VoucherApproval(List<OptionNos> list)
        {
            Dictionary<int, CheckSessionData> IDictionary = CheckSessionData.GetSessionValues();
            long companyId = (long)IDictionary[1].Id;
            long locationId = (long)IDictionary[2].Id;
            long userId = (long)IDictionary[3].Id;

            Result result=new Result();
            List<Voucher>vouchers=new List<Voucher>();

            foreach (OptionNos option in list)
            {
                vouchers.Add(new Voucher{VoucherNo = option.OptionNo});
            }

            result = _voucherManager.UpdateVoucher(vouchers, userId);
            return result;
        }
	}
}