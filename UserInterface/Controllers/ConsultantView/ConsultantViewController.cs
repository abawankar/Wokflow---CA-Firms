using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UserInterface.Models.Master;
using UserInterface.Models.Transaction;

namespace UserInterface.Controllers.ConsultantView
{
    public class ConsultantViewController : ApplicationBaseController
    {
        private static log4net.ILog Log { get; set; }
        ILog log = log4net.LogManager.GetLogger(typeof(ConsultantViewController));

        // GET: ConsultantView
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult List(string name = null, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                TaskManagerRepository dal = new TaskManagerRepository();
                List<TaskManagerModel> model = new List<TaskManagerModel>();
                model = dal.GetByConsultant(ConsultantRepository.GetByUserName(User.Identity.Name)).ToList();
                int count = model.Count;
                var Model1 = model.Skip(jtStartIndex).Take(jtPageSize).ToList();
                return Json(new { Result = "OK", Records = Model1, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        public ActionResult ConsultantTodayTask()
        {
            return PartialView();
        }

        public JsonResult TodayTaskList()
        {
            try
            {
                TaskManagerRepository dal = new TaskManagerRepository();
                List<TaskManagerModel> model = new List<TaskManagerModel>();
                model = dal.GetByConsultant(ConsultantRepository.GetByUserName(User.Identity.Name)).ToList();
                model = model.Where(x => x.Start.ToShortDateString() == System.DateTime.Today.ToShortDateString()).ToList();

                return Json(new { Result = "OK", Records = model });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        public JsonResult PendingTaskList()
        {
            try
            {
                TaskManagerRepository dal = new TaskManagerRepository();
                List<TaskManagerModel> model = new List<TaskManagerModel>();
                model = dal.GetByConsultantPending(ConsultantRepository.GetByUserName(User.Identity.Name)).ToList();
                return Json(new { Result = "OK", Records = model });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }
    }
}