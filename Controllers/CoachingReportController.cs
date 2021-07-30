using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CoachingEntity.Model;
using CoachingManagementSystem.CoachingEntity.CommonModel;
using CoachingManagementSystem.Helper;
using CoachingManagementSystem.Models;
using CoachingManagementSystem.Models.CommonModel;
using CoachingManager.Manager;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf;
using Rotativa;

namespace CoachingManagementSystem.Controllers
{
    public class CoachingReportController : Controller
    {
        StudentManager _studentManager = new StudentManager();
        VoucherManager _voucherManager=new VoucherManager();
        CommonManager commonManager = new CommonManager();
        #region Prev Code
        //
        // GET: /CoachingReport/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult _PaymentReceiptReport(long ReceiptId)
        {
            Dictionary<int, CheckSessionData> IDictionary = CheckSessionData.GetSessionValues();
            long companyId = (long)IDictionary[1].Id;
            long locationId = (long)IDictionary[2].Id;
            long userId = (long)IDictionary[3].Id;
            string userName = (string)IDictionary[3].Name;
            //long receiptId = Session["ReceiptId"] != null ? (long) Session["ReceiptId"] : 0;
            StudentPaymentDetails studentPaymentDetails = _studentManager.SelectStudentPaymentDetailsByReceiptNo(new StudentPaymentDetails { StudentPaymentId = ReceiptId }).FirstOrDefault();



            if (studentPaymentDetails != null)
            {
                Student student = _studentManager.SelectStudent(new Student { StudentId = studentPaymentDetails.StudentId });
                List<StudentPaymentDetails> studentPaymentDetailses = _studentManager.SelectStudentPaymentDetails(new Student { StudentCode = student.StudentCode });
                PaymentReceiptVM paymentReceiptVm = new PaymentReceiptVM
                {
                    PaymentAmount = studentPaymentDetails.PaymentAmount,
                    AdmissionFee = student.AdmissionFee,
                    Discount = student.Discount,
                    DepartmentName = student.Department.Name,
                    NextPaymentDate = studentPaymentDetails.NextPaymentDate,
                    ReceiptNo = studentPaymentDetails.PaymentReceiptNo,
                    StudentName = student.Name,
                    Vat = student.Vat,
                    Due = (student.AdmissionFee - studentPaymentDetailses.Sum(x => x.PaymentAmount) - student.Discount),
                    StudentRoll = student.StudentCode,
                    PaymentDate = studentPaymentDetails.PaymentDate
                };

                return View(paymentReceiptVm);
            }
            else
            {
                return View(new PaymentReceiptVM());
            }

        }
        public ActionResult StudentPaymentReceipt(long ReceiptId)
        {
            return new ActionAsPdf("_PaymentReceiptReport", new { ReceiptId = ReceiptId });//, new { name = "Giorgio" } { FileName = "Test.pdf" }
        }

        public ActionResult StudentPaymentReceiptFromStudentCreate(string PaymentOptionNo)
        {

            StudentPaymentDetails studentPaymentDetails = _studentManager.SelectStudentPaymentDetailsByReceiptNo(new StudentPaymentDetails { PaymentReceiptNo = PaymentOptionNo }).FirstOrDefault();
            return new ActionAsPdf("_PaymentReceiptReport", new { ReceiptId = studentPaymentDetails.StudentPaymentId });//, new { name = "Giorgio" } { FileName = "Test.pdf" }
        }
        //public ActionResult StudentPaymentReceipt(PaymentReceiptVM paymentReceiptVm)
        //{
        //    ITextSharpBasedReport _obj = new ITextSharpBasedReport();
        //    ArrayList a_list = new ArrayList();

        //    string docType = "PDF";
        //    _obj.GetDocument(docType, "StudentPaymentInvoice", a_list);

        //    var htmlContent = "<h1>Mr.Mamun</hi>";

        //   // var model = new PdfInfo { Content = htmlContent, Name = "PDF Doc" };
        //    //return new ViewAsPdf(model);

        //}

        //
        //    a_list.Add(BuyerId);
        //    a_list.Add(DateFrom);
        //    a_list.Add(DateTo);

        [HttpGet]
        public void StudentPaymentReceipt(long ReceiptId, string docType)
        {

            ITextSharpBasedReport _obj = new ITextSharpBasedReport();
            ArrayList a_list = new ArrayList();
            a_list.Add(ReceiptId);
            docType = "PDF";
            _obj.GetDocument(docType, "StudentPaymentInvoice", a_list);
        }
        public ActionResult StudentIdCard()
        {
            return View();
        }
        public ActionResult DailyCollectionReport()
        {
            return View();
        }

        public ActionResult AccountsReport()
        {
            return View();
        }
        public ActionResult IndividualReport()
        {
            return View();
        }

        public ActionResult RevenueReport()
        {
            return View();
        }
        public ActionResult ExpenseReport()
        {
            return View();
        }

        public void ViewRevenueReport(string fromDate, string toDate, string accId = "")
        {
            ITextSharpBasedReport _obj = new ITextSharpBasedReport();
            ArrayList a_list = new ArrayList();
            List<string> list = new List<string>();

            list.Add(fromDate);
            list.Add(toDate);
            list.Add(accId);
            string docType = "PDF";
            _obj.GetDocument(docType, "RevenueReport", a_list, list);
        }
        public void ViewExpenseReport(string fromDate, string toDate, string accId = "")
        {
            ITextSharpBasedReport _obj = new ITextSharpBasedReport();
            ArrayList a_list = new ArrayList();
            List<string> list = new List<string>();

            list.Add(fromDate);
            list.Add(toDate);
            string docType = "PDF";
            _obj.GetDocument(docType, "ExpenseReport", a_list, list);
        }
        public void DailyCollectionDetailReport(string fromDate, string toDate, string studentId)
        {
            ITextSharpBasedReport _obj = new ITextSharpBasedReport();
            ArrayList a_list = new ArrayList();
            List<string> list = new List<string>();
            if (studentId == "")
            {
                list.Add(fromDate);
                list.Add(toDate);
            }
            else
            {
                list.Add(studentId);
            }

            string docType = "PDF";
            _obj.GetDocument(docType, "DateWiseFeeCollectionReport", a_list, list);
        }


        public void ShowReport(List<string> listItems)
        {
            ITextSharpBasedReport _obj = new ITextSharpBasedReport();
            ArrayList a_list = new ArrayList();
            List<string> list = new List<string>();
            string[] items = listItems[0].Split(',');
            foreach (string item in items)
            {
                list.Add(item);
            }

            string docType = "PDF";
            _obj.GetDocument(docType, "IndividualVoucherReport", a_list, list);
        }
        public JsonResult LoadDataIdCardPrint()
        {

            List<CoachingSession> coachingSes = new List<CoachingSession>();
            coachingSes = commonManager.GetActiveSessions().ToList();

            List<Student> list = new List<Student>();
            list = _studentManager.SelectStudents(new Student { Status = true, SessionId = coachingSes[0].SessionId });
            var studentList = list.Select(x => new { Name = x.Name, Batch = x.Batch == null ? "Not Assigned" : x.Batch.Name, Department = x.Department.Name, Code = x.StudentCode, StdId = x.Id }).OrderBy(x => x.Code).ToList();
            return Json(studentList.ToArray(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult StudentListReport()
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
            var items = list.Select(x => new { OptionNo = x.OptionNo }).ToList();
            return Json(items.ToArray(), JsonRequestBehavior.AllowGet);

        }

        private List<OptionNos> GetVoucherList(string fromDate, string toDate)
        {
            List<Voucher> list = new List<Voucher>();
            list = _voucherManager.SelectVouchers().ToList();
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

            return list.Select(x => new OptionNos { OptionId = x.VoucherId.ToString(), OptionNo = x.VoucherNo, Amount = x.VoucherDetails.Sum(c => c.Credit) }).ToList();
        }
        public void PrintIdCard(List<string> stdList)
        {
            ITextSharpBasedReport _obj = new ITextSharpBasedReport();
            ArrayList a_list = new ArrayList();

            string docType = "PDF";
            _obj.GetDocument(docType, "StudentIdCard", a_list, stdList);


        }
        public void PrintStudentList(List<string> stdList)
        {
            ITextSharpBasedReport _obj = new ITextSharpBasedReport();
            ArrayList a_list = new ArrayList();
            stdList = (List<string>)Session["studentCodelist"];
            string docType = "PDF";
            _obj.GetDocument(docType, "PrintStudentListInfo", a_list, stdList);


        }

        [HttpPost]
        public JsonResult CreateSessionStudentCodeList(List<string> stdCodeList)
        {
            if (stdCodeList != null && stdCodeList.Count > 0)
                Session["studentCodelist"] = stdCodeList;
            return Json("ok");
        }

        #endregion

        //public ActionResult ShowReport()
        //{
        //    return View();
        //}

    }
}