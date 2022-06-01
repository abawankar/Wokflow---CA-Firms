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
    public class ReceivableController : ApplicationBaseController
    {
        private static log4net.ILog Log { get; set; }
        ILog log = log4net.LogManager.GetLogger(typeof(ReceivableController));

        public ActionResult Index()
        {
            if (User.IsInRole("Admin") || EmpRightsRepository.RightList(User.Identity.Name).Contains("R010"))
            {
                return View();
            }
            else
            {
                return View("Security");
            }
        }

        [HttpPost]
        public JsonResult List(int nboid = 0)
        {
            try
            {
                ReceivableRepository dal = new ReceivableRepository();
                List<ReceivableModel> model = new List<ReceivableModel>();
                model = dal.GetReceivable(nboid).ToList();
                return Json(new { Result = "OK", Records = model });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Create(ReceivableModel model, int nboid)
        {
            try
            {
                if (User.IsInRole("Admin") || EmpRightsRepository.RightList(User.Identity.Name).Contains("R011"))
                {
                    ReceivableRepository dal = new ReceivableRepository();
                    dal.InsertReceivable(model, nboid);
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
        public JsonResult Update(ReceivableModel model)
        {
            try
            {
                if (User.IsInRole("Admin") || EmpRightsRepository.RightList(User.Identity.Name).Contains("R011"))
                {
                    ReceivableRepository dal = new ReceivableRepository();
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
        public JsonResult Delete(ReceivableModel model)
        {
            try
            {
                if (User.IsInRole("Admin") || EmpRightsRepository.RightList(User.Identity.Name).Contains("R011"))
                {
                    ReceivableRepository dal = new ReceivableRepository();
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
    }
}