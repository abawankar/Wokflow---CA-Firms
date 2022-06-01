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

namespace UserInterface.Controllers.Report
{
    public class EmployeeAssignedClientController : ApplicationBaseController
    {
        private static List<ClientToEmployee> exportlist = new List<ClientToEmployee>();
        private static log4net.ILog Log { get; set; }
        ILog log = log4net.LogManager.GetLogger(typeof(EmployeeAssignedClientController));

        // GET: EmployeeAssignedClient
        public ActionResult Index()
        {
            if (User.IsInRole("Admin"))// || EmpRightsRepository.RightList(User.Identity.Name).Contains("R006"))
            {
                EmployeeRepository emp = new EmployeeRepository();
                ViewBag.emp = new SelectList(emp.GetAll().OrderBy(x => x.Name), "Id", "Name");
                return View();
            }
            else
            {
                return View("Security");
            }
        }

        [HttpPost]
        public JsonResult List(int jtStartIndex = 0,int empid =0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                ClientRepository dal = new ClientRepository();
                EmployeeRepository emp = new EmployeeRepository();

                var data = from e in emp.GetAll().Where(x=>x.Client.Count != 0)
                           from c in e.Client
                           select new ClientToEmployee {Id=c.Id, EmpName = e.Name, ClientName = c.Name,EmpId=e.Id };

                var clients = emp.GetAll().SelectMany(x => x.Client);
                var clientid = from c in clients
                               select c.Id;

                var list = from m in dal.GetAll().Where(c => !clientid.Contains(c.Id)).ToList()
                           select new ClientToEmployee {Id=m.Id, EmpName = "", ClientName = m.Name, EmpId = 0 };

                var model = data.Concat(list).ToList();

                if (empid != 0)
                {
                    model = model.Where(x => x.EmpId == empid).ToList();
                }
                var count = model.Count();
                model = model.OrderBy(x => x.ClientName).ToList();
                exportlist = model.ToList();
                List<ClientToEmployee> Model1 = model.Skip(jtStartIndex).Take(jtPageSize).ToList();
                return Json(new { Result = "OK", Records = Model1, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        public ActionResult Export()
        {
            try
            {
                var data = exportlist.ToList();
                ReportDataSet rds = new ReportDataSet();
                foreach (var item in data.OrderBy(x=>x.ClientName))
                {
                    DataRow dr = rds.Tables["ClientToEmployee"].NewRow();
                    dr["Id"] = item.Id;
                    dr["ClientName"] = item.ClientName;
                    dr["EmpName"] = item.EmpName;
                    rds.Tables["ClientToEmployee"].Rows.Add(dr);
                }
                
                ReportDocument rd = new ReportDocument();
                var reportpath = Server.MapPath(Url.Content("~/Reports/ClientToEmployee.rpt"));
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