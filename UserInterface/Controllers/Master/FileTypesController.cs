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

    public class FileTypesController : ApplicationBaseController
    {
        private static log4net.ILog Log { get; set; }
        ILog log = log4net.LogManager.GetLogger(typeof(FileTypesController));

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
                FileTypesRepository dal = new FileTypesRepository();
                List<FileTypesModel> model = new List<FileTypesModel>();
                if (string.IsNullOrEmpty(name))
                {
                    model = dal.GetAll().ToList();
                }
                else
                {
                    model = dal.GetAll().ToList();
                }
                int count = model.Count;
                List<FileTypesModel> Model1 = model.Skip(jtStartIndex).Take(jtPageSize).ToList();
                return Json(new { Result = "OK", Records = Model1, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }
        
        
        [HttpPost]
        public JsonResult Create(FileTypesModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }
                FileTypesRepository dal = new FileTypesRepository();
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
        public JsonResult Update(FileTypesModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }
                FileTypesRepository dal = new FileTypesRepository();
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
        public JsonResult GetFileTypesOptions()
        {
            List<FileTypesModel> model = new List<FileTypesModel>();
            model.Add(new FileTypesModel { Id = 0, Name = "* Select *" });
            var data = model.Select(c => new { DisplayText = c.Name, Value = c.Id });
            try
            {
                FileTypesRepository dal = new FileTypesRepository();
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