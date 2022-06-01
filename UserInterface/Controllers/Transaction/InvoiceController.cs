using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UserInterface.Models.Master;
using UserInterface.Models.Transaction;
using UserInterface.Models;
using CrystalDecisions.CrystalReports.Engine;

namespace UserInterface.Controllers.Transaction
{
    public class InvoiceController : ApplicationBaseController
    {
        private static log4net.ILog Log { get; set; }
        ILog log = log4net.LogManager.GetLogger(typeof(InvoiceController));
        // GET: Invoice
        public ActionResult Index()
        {
            if (User.IsInRole("Admin") || EmpRightsRepository.RightList(User.Identity.Name).Contains("R012"))
            {
                var data = ClientRepository.ClientOptions().OrderBy(x => x.Name);
                ViewBag.client = new SelectList(data, "Id", "Name");

                CompanyRepository dal = new CompanyRepository();
                ViewBag.comp = new SelectList(dal.GetAll(), "Id", "Name");
                return View();
            }
            else
            {
                return View("Security");
            }
        }

        [HttpPost]
        public JsonResult List(string name = null, int client = 0,int comp=0, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                InvoiceRepository dal = new InvoiceRepository();
                List<InvoiceModel> model = new List<InvoiceModel>();
                if (string.IsNullOrEmpty(name))
                {
                    model = dal.GetAll().ToList();
                }
                else
                {
                    model = dal.GetAll().Where(x => (x.InvoiceNo + " " + x.TotalAmount).Contains(name)).ToList();
                }
                if (comp != 0)
                {
                    model = model.Where(x => x.Company.Id == comp).ToList();
                }
                if (client != 0)
                {
                    model = model.Where(x => x.Client.Id == client).ToList();
                }
                
                int count = model.Count;
                model = model.OrderByDescending(x => x.Date).ToList();
                List<InvoiceModel> Model1 = model.Skip(jtStartIndex).Take(jtPageSize).ToList();
                return Json(new { Result = "OK", Records = Model1, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Create(InvoiceModel model)
        {
            try
            {
                if (User.IsInRole("Admin") || EmpRightsRepository.RightList(User.Identity.Name).Contains("R012"))
                {
                    if (!ModelState.IsValid)
                    {
                        return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                    }
                    InvoiceRepository dal = new InvoiceRepository();
                    dal.Insert(model);
                    return Json(new { Result = "OK", Record = model });
                }
                else
                {
                    return Json(new { Result = "Error", Message = "Sorry, You are not Authorized to do this action" });
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Update(InvoiceModel model)
        {
            try
            {
                if (User.IsInRole("Admin") || EmpRightsRepository.RightList(User.Identity.Name).Contains("R012"))
                {
                    if (!ModelState.IsValid)
                    {
                        return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                    }
                    InvoiceRepository dal = new InvoiceRepository();
                    dal.Edit(model);
                    model = dal.GetById(model.Id);
                    return Json(new { Result = "OK", Record = model });
                }
                else
                {
                    return Json(new { Result = "Error", Message = "Sorry, You are not Authorized to do this action" });
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult InvTrnList(int invid)
        {
            try
            {
                InvoiceTrnRepository dal = new InvoiceTrnRepository();
                var model = dal.GetAll(invid);
                int count = model.Count;
                return Json(new { Result = "OK", Records = model });
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult UpdateTrn(InvoiceTrnModel model)
        {
            try
            {
                if (User.IsInRole("Admin") || User.IsInRole("Accountant"))
                {
                    InvoiceTrnRepository dal = new InvoiceTrnRepository();
                    dal.Edit(model);
                    return Json(new { Result = "OK", Record = model });
                }
                else
                {
                    return Json(new { Result = "Error", Message = "Sorry, You are not Authorized to do this action" });
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult DeleteTrn(InvoiceTrnModel model)
        {
            try
            {
                if (User.IsInRole("Admin") || User.IsInRole("Accountant"))
                {
                    InvoiceTrnRepository dal = new InvoiceTrnRepository();
                    dal.Delete(model.Id);
                    return Json(new { Result = "OK", Record = model });
                }
                else
                {
                    return Json(new { Result = "Error", Message = "Sorry, You are not Authorized to do this action" });
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult AddInvTrn(InvoiceTrnModel model, int invid)
        {
            try
            {
                if (User.IsInRole("Admin") || User.IsInRole("Accountant"))
                {
                    InvoiceTrnRepository dal = new InvoiceTrnRepository();
                    dal.Insert(model, invid);
                    return Json(new { Result = "OK", Record = model });
                }
                else
                {
                    return Json(new { Result = "Error", Message = "Sorry, You are not Authorized to do this action" });
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        public ActionResult PrintInvoice(int id)
        {
            try
            {
                InvoiceRepository dal = new InvoiceRepository();
                var data = dal.GetById(id);
                ReportDataSet rds = new ReportDataSet();
                DataRow dr = rds.Tables["InvoiceHeader"].NewRow();
                dr["Id"] = data.Id;
                dr["InvoiceNo"] = data.InvoiceNo;
                dr["Date"] = data.Date.ToString("dd MMM yyyy");
                dr["ServiceTax"] = data.SerTaxAmount;
                dr["TDS"] = data.TDSAmount;
                dr["InvoiceAmount"] = data.InvTrn.Sum(x => x.Amount);
                dr["AmountInWords"] = (data.NetAmount).ToString().SpellNumber();
                dr["Client"] = data.Client.Name;
                dr["ClientAddress"] = data.Client.Address;
                dr["NetAmount"] = data.NetAmount;

                dr["SerTax%"] = data.SerTax;
                dr["TDS%"] = data.TDS;

                rds.Tables["InvoiceHeader"].Rows.Add(dr);

                foreach (var item in data.InvTrn)
                {
                    DataRow drt = rds.Tables["InvoiceBody"].NewRow();
                    drt["Id"] = item.Id;
                    drt["InvId"] = data.Id;
                    drt["Perticulars"] = item.Particulars;
                    drt["Amount"] = item.Amount;
                    rds.Tables["InvoiceBody"].Rows.Add(drt);
                }

                ReportDocument rd = new ReportDocument();
                if(data.CompId ==1){
                    var reportpath = Server.MapPath(Url.Content("~/Reports/Invoice.rpt"));
                    rd.Load(reportpath);
                }

                if (data.CompId == 2)
                {
                    var reportpath = Server.MapPath(Url.Content("~/Reports/Invoice2.rpt"));
                    rd.Load(reportpath);
                }
                
                rd.SetDataSource(rds);
                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();
                try
                {
                    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/pdf", "Invoice.pdf");
                }
                catch (Exception ex)
                {
                    //Log.Error(ex.Message);
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                //Log.Error(ex.Message);
                throw ex;
            }
        }

        public ActionResult CompletedTaskList(int invid,int clientid)
        {
            var data = ClientRepository.ClientOptions().Where(x => x.Id == clientid).ToList();
            ViewBag.client = new SelectList(data, "Id", "Name");
            ViewBag.invid = invid;
            return PartialView();
        }

        [HttpPost]
        public JsonResult ListOfTask(int id = 0, int jtStartIndex = 0, int jtPageSize = 0)
        {
            NBORepository dal = new NBORepository();
            try
            {
                var list = dal.GetCompltedTask().Where(x => x.Client.Id == id).ToList();

                int count = list.Count;
                var Model1 = list.Skip(jtStartIndex).Take(jtPageSize).ToList();
                return Json(new { Result = "OK", Records = Model1, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult AddTask(string id, string list)
        {
            InvoiceRepository.AddTask(Convert.ToInt32(id), list);
            return RedirectToAction("Index");
        }
    }
}