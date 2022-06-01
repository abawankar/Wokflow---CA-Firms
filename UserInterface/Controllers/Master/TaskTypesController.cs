using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ApplicationServices.Web;
using UserInterface.Models.Master;
using log4net;

namespace UserInterface.Controllers.Master
{[HandleError]

    public class TaskTypesController : ApplicationBaseController
    {
        private static log4net.ILog Log { get; set; }
        ILog log = log4net.LogManager.GetLogger(typeof(TaskTypesController));

        // GET: FileTypes
        
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult List(string name = null, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                TaskTypesRepository dal = new TaskTypesRepository();
                List<TaskTypesModel> model = new List<TaskTypesModel>();
                if (string.IsNullOrEmpty(name))
                {
                    model = dal.GetAll().ToList();
                }
                else
                {
                    model = dal.GetAll().Where(x=>x.Name.ToLower().Contains(name.ToLower())).ToList();
                }
                int count = model.Count;
                model = model.OrderBy(x => x.Name).ToList();
                List<TaskTypesModel> Model1 = model.Skip(jtStartIndex).Take(jtPageSize).ToList();
                return Json(new { Result = "OK", Records = Model1, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }
        
        [HttpPost]
        public JsonResult Create(TaskTypesModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }
                TaskTypesRepository dal = new TaskTypesRepository();
                dal.Insert(model);
                return Json(new { Result = "OK", Record = model });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Update(TaskTypesModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }
                TaskTypesRepository dal = new TaskTypesRepository();
                dal.Edit(model);
                return Json(new { Result = "OK", Record = model });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Delete(TaskTypesModel model)
        {
            try
            {
                TaskTypesRepository dal = new TaskTypesRepository();
                var data = dal.Delete(model.Id);
                return Json(new { Result = "OK", Record = data });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult GetTaskTypesOptions()
        {
            List<TaskTypesModel> model = new List<TaskTypesModel>();
            model.Add(new TaskTypesModel { Id = 0, Name = "* Select *" });
            var data = model.Select(c => new { DisplayText = c.Name, Value = c.Id });
            try
            {
                TaskTypesRepository dal = new TaskTypesRepository();
                var list = dal.GetAll()
                                .Select(c => new { DisplayText = c.Name, Value = c.Id });
                return Json(new { Result = "OK", Options = list.Concat(data).OrderBy(x=>x.DisplayText) });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
    }
}