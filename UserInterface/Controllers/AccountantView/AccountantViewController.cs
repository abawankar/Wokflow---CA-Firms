using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UserInterface.Models.Master;
using UserInterface.Models.Transaction;

namespace UserInterface.Controllers.AccountantView
{
    public class AccountantViewController : ApplicationBaseController
    {
        private static log4net.ILog Log { get; set; }
        ILog log = log4net.LogManager.GetLogger(typeof(AccountantViewController));

        // GET: AccountantView
        public ActionResult Index()
        {
            ClientRepository dal = new ClientRepository();
            var data = from m in dal.GetAll()
                       orderby m.Name
                       select new { Id = m.Id, Name = m.Name };
            ViewBag.client = new SelectList(data, "Id", "Name");

            TaskTypesRepository file = new TaskTypesRepository();
            ViewBag.tasktype = new SelectList(file.GetAll().OrderBy(x => x.Name), "Id", "Name");

            return PartialView();
        }

        [HttpPost]
        public JsonResult List(string name = null, int client = 0, int tasktype = 0, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                NBORepository dal = new NBORepository();
                List<NBOModel> model = new List<NBOModel>();
                if (string.IsNullOrEmpty(name))
                {
                    model = dal.GetCompltedTask().ToList();
                }
                else
                {
                    model = dal.GetCompltedTask().ToList();
                }
                if (client != 0)
                {
                    model = model.Where(x => x.Client.Id == client).ToList();
                }
                if (tasktype != 0)
                {
                    model = model.Where(x => x.TaskTypes.Id == tasktype).ToList();
                }
                if (!string.IsNullOrEmpty(name))
                {
                    model = model.Where(x => x.FileNumber == Convert.ToInt32(name)).ToList();
                }
                int count = model.Count;
                List<NBOModel> Model1 = model.Skip(jtStartIndex).Take(jtPageSize).ToList();
                return Json(new { Result = "OK", Records = Model1, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Update(NBOModel model)
        {
            try
            {
                if (User.IsInRole("Accountant"))
                {
                    NBORepository dal = new NBORepository();
                    dal.EditForAccountant(model);
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
    }
}