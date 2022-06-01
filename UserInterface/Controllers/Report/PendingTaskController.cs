using CrystalDecisions.CrystalReports.Engine;
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

namespace UserInterface.Controllers.Report
{
    public class PendingTaskController : ApplicationBaseController
    {
        private static List<NBOModel> exportlist = new List<NBOModel>();
        private static log4net.ILog Log { get; set; }
        ILog log = log4net.LogManager.GetLogger(typeof(PendingTaskController));

        // GET: PendingTask
        public ActionResult Index()
        {
            ViewBag.client = new SelectList(ClientRepository.ClientOptions().OrderBy(x => x.Name), "Id", "Name");

            TaskTypesRepository file = new TaskTypesRepository();
            ViewBag.tasktype = new SelectList(file.GetAll().OrderBy(x => x.Name), "Id", "Name");

            ViewBag.fy = new SelectList(new[] { 
                new SelectListItem{Value="2016-17", Text="2016-17"},
                }, "Value", "Text");

            return View();
        }

        public JsonResult List(string FY = null, int client = 0, int tasktype = 0, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                NBORepository dal = new NBORepository();
                List<NBOModel> model = new List<NBOModel>();
                model = dal.GetPendingTask().ToList();
                if (client != 0)
                {
                    model = model.Where(x => x.Client.Id == client).ToList();
                }
                if (tasktype != 0)
                {
                    model = model.Where(x => x.TaskTypes.Id == tasktype).ToList();
                }
                int count = model.Count;
                exportlist = model.ToList();
                List<NBOModel> Model1 = model.Skip(jtStartIndex).Take(jtPageSize).ToList();
                return Json(new { Result = "OK", Records = Model1, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        public ActionResult Export()
        {
            try
            {
                var data = exportlist.ToList();
                ReportDataSet rds = new ReportDataSet();
                foreach (var item in data.OrderBy(x => x.Received))
                {
                    DataRow dr = rds.Tables["Task"].NewRow();
                    dr["Id"] = item.Id;
                    dr["FY"] = item.FY;
                    dr["FileNumber"] = item.FileNumber;
                    dr["TaskTypes"] = item.TaskTypes.Name;
                    dr["Client"] = item.Client.Name;
                    dr["Received"] = item.Received;
                    dr["DueDate"] = item.DueDate;
                    dr["Cost"] = item.Cost;
                    dr["Bill"] = item.BillStatus==1?"Pending":item.BillStatus==2?"Done":"";
                    dr["Filing"] = item.FilingStatus == 1 ? "Pending" : item.FilingStatus == 2 ? "Done" : "";
                    dr["Payment"] = item.PaymentStatus == 1 ? "Pending" : item.PaymentStatus == 2 ? "Paid" : "";
                    dr["OfficeFile"] = item.OfficeFileNo;
                    dr["Status"] = item.Status == 1 ? "Run" : item.Status == 3 ? "Can" : item.Status == 4?"Closed":"Done";
                    rds.Tables["Task"].Rows.Add(dr);
                }

                ReportDocument rd = new ReportDocument();
                var reportpath = Server.MapPath(Url.Content("~/Reports/PendingTask.rpt"));
                rd.Load(reportpath);
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
    }
}