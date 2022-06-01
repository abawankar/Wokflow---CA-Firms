using ApplicationServices.Web;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UserInterface.Models.Master;
using UserInterface.Models.Transaction;

namespace UserInterface.Controllers.Transaction
{
    [HandleError]
    public class TaskManagerController : ApplicationBaseController
    {
        private static log4net.ILog Log { get; set; }
        ILog log = log4net.LogManager.GetLogger(typeof(TaskManagerController));

        // GET: TaskManager
        public ActionResult Index()
        {
            ClientRepository dal = new ClientRepository();
            var data = from m in dal.GetAll()
                       orderby m.Name
                       select new { Id = m.Id, Name = m.Name };
            ViewBag.client = new SelectList(data, "Id", "Name");

            ViewBag.consultant = new SelectList(ConsultantRepository.ConsultantOptions().OrderBy(x=>x.Name), "Id", "Name");

            TaskTypesRepository file = new TaskTypesRepository();
            ViewBag.tasktype = new SelectList(file.GetAll().OrderBy(x => x.Name), "Id", "Name");

            return View();
        }

        [HttpPost]
        public JsonResult Update(TaskManagerModel model)
        {
            try
            {
                TaskManagerRepository dal = new TaskManagerRepository();
                dal.Edit(model);
                model = dal.GetById(model.Id);
                return Json(new { Result = "OK", Record = model });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        public ActionResult TodayTask()
        {
            return PartialView();
        }

        [HttpPost]
        public JsonResult List(string name = null, int client =0, int tasktype =0,int consultant =0, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                TaskManagerRepository dal = new TaskManagerRepository();
                List<TaskManagerModel> model = new List<TaskManagerModel>();
                if(User.IsInRole("User"))
                {
                    model = dal.GetTodayTask(EmployeeRepository.GetByUserName(User.Identity.Name)).ToList();
                }
                if(User.IsInRole("Admin"))
                {
                    model = dal.GetAll().ToList();
                }

                if(tasktype != 0)
                {
                    model = model.Where(x => x.FileNumber.TaskTypes.Id == tasktype).ToList();
                }
                if(client != 0)
                {
                    model = model.Where(x => x.FileNumber.Client.Id == client).ToList();
                }
                if (consultant != 0)
                {
                    model = model.Where(x => x.Consultant != null && x.Consultant.Id == consultant).ToList();
                }
                if(!string.IsNullOrEmpty(name))
                {
                    model = model.Where(x => (x.FileNumber.FileNumber + " " + x.Notes).ToLower().Contains(name.ToLower())).ToList();
                }
               
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

        [HttpPost]
        public JsonResult CommentsList(int taskid)
        {
            try
            {
                TaskCommentsRepository dal = new TaskCommentsRepository();
                var model = dal.GetAll(taskid);
                return Json(new { Result = "OK", Records = model });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }
        [HttpPost]
        public JsonResult AddComments(TaskCommentsModel model, int taskid)
        {
            model.Date = System.DateTime.Today;
            model.UserName = User.Identity.Name;
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }
                TaskCommentsRepository dal = new TaskCommentsRepository();
                dal.Insert(model, taskid);
                return Json(new { Result = "OK", Record = model });

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult UpdateComments(TaskCommentsModel model)
        {
            model.Date = System.DateTime.Today;
            model.UserName = User.Identity.Name;
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }
                TaskCommentsRepository dal = new TaskCommentsRepository();
                if (User.Identity.Name.ToLower() == dal.GetById(model.Id).UserName.ToLower())
                {
                    dal.Edit(model);
                    return Json(new { Result = "OK", Record = model });
                }
                else
                {
                    return Json(new { Result = "Error", Message = "Sorry, You can edit only your comments, not others" });
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        public JsonResult TodayTaskList()
        {
            try
            {
                TaskManagerRepository dal = new TaskManagerRepository();
                List<TaskManagerModel> model = new List<TaskManagerModel>();
                if(User.IsInRole("User"))
                {
                    model = dal.GetTodayTask(EmployeeRepository.GetByUserName(User.Identity.Name) ).ToList();
                    model = model.Where(x => x.Start.ToShortDateString() == System.DateTime.Today.ToShortDateString()).ToList();
                }

                if(User.IsInRole("Admin"))
                {
                    model = dal.GetTodayTask().ToList();
                }
                
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
                if(User.IsInRole("User"))
                {
                    model = dal.GetPendingTask(EmployeeRepository.GetByUserName(User.Identity.Name)).ToList();
                }
                if (User.IsInRole("Admin"))
                {
                    model = dal.GetPendingTask().ToList();
                }
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