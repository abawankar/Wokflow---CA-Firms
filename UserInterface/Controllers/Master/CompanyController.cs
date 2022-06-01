using ApplicationServices.Web;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UserInterface.Models.Master;

namespace UserInterface.Controllers.Master
{
    [HandleError]
    [ControllerAuthorize]
    public class CompanyController : ApplicationBaseController
    {
        private static log4net.ILog Log { get; set; }
        ILog log = log4net.LogManager.GetLogger(typeof(CompanyController));

        [ActionAuthorize]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult List(string name = null, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                CompanyRepository dal = new CompanyRepository();
                List<CompanyModel> model = new List<CompanyModel>();
                if (string.IsNullOrEmpty(name))
                {
                    model = dal.GetAll().ToList();
                }
                else
                {
                    model = dal.GetAll().ToList();
                }
                int count = model.Count;
                List<CompanyModel> Model1 = model.Skip(jtStartIndex).Take(jtPageSize).ToList();
                return Json(new { Result = "OK", Records = Model1, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [ActionAuthorize]
        [HttpPost]
        public JsonResult Create(CompanyModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }
                CompanyRepository dal = new CompanyRepository();
                dal.Insert(model);
                return Json(new { Result = "OK", Record = model });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [ActionAuthorize]
        [HttpPost]
        public JsonResult Update(CompanyModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }
                CompanyRepository dal = new CompanyRepository();
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
        public JsonResult GetCompanyOptions()
        {
            List<CompanyModel> model = new List<CompanyModel>();
            model.Add(new CompanyModel { Id = 0, Name = "* Select *" });
            var data = model.Select(c => new { DisplayText = c.Name, Value = c.Id });
            try
            {
                CompanyRepository dal = new CompanyRepository();
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