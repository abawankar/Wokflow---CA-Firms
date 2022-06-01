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
    public class ConsultantController : ApplicationBaseController
    {
        private static log4net.ILog Log { get; set; }
        ILog log = log4net.LogManager.GetLogger(typeof(ConsultantController));
        // GET: Consultant
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult List(string name = null, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                ConsultantRepository dal = new ConsultantRepository();
                List<ConsultantModel> model = new List<ConsultantModel>();
                if (string.IsNullOrEmpty(name))
                {
                    model = dal.GetAll().ToList();
                }
                else
                {
                    model = dal.GetAll().Where(x=>(x.Name + " " + x.PAN).ToLower().Contains(name.ToLower())) .ToList();
                }
                int count = model.Count;
                List<ConsultantModel> Model1 = model.Skip(jtStartIndex).Take(jtPageSize).ToList();
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
        public JsonResult Create(ConsultantModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }
                ConsultantRepository dal = new ConsultantRepository();
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
        public JsonResult Update(ConsultantModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }
                ConsultantRepository dal = new ConsultantRepository();
                dal.Edit(model);
                return Json(new { Result = "OK", Record = model });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        public ActionResult AddClients(string id, string list)
        {
            ConsultantRepository.AddClients(Convert.ToInt32(id), list);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult GetClients(int id = 0)
        {
            ConsultantRepository dal = new ConsultantRepository();
            try
            {
                var data = dal.GetById(id).Clients.ToList();

                return Json(new { Result = "OK", Records = data });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult ListOfClients(int id = 0,string name=null, int jtStartIndex = 0, int jtPageSize = 0)
        {
            ConsultantRepository dal = new ConsultantRepository();
            ClientRepository cdal = new ClientRepository();
            try
            {
                var clients = dal.GetAll().SelectMany(x => x.Clients);
                var clientid = from c in clients
                             select c.Id;

                var list = cdal.GetAll().Where(c => !clientid.Contains(c.Id)).ToList();
                if (!string.IsNullOrEmpty(name))
                {
                    list = list.Where(x => x.Name.ToLower().Contains(name.ToLower())).ToList();
                }

                int count = list.Count;
                list = list.OrderBy(x => x.Name).ToList();
                var Model1 = list.Skip(jtStartIndex).Take(jtPageSize).ToList();
                return Json(new { Result = "OK", Records = Model1, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult GetClientsOptions(int id = 0)
        {
            ConsultantRepository dal = new ConsultantRepository();
            ClientRepository cdal = new ClientRepository();
            try
            {
                if (id == 0)
                {
                    var curr = cdal.GetAll()
                               .Select(c => new { DisplayText = c.Name, Value = c.Id });
                    return Json(new { Result = "OK", Options = curr });
                }
                else
                {
                    var curr = dal.GetById(id).Clients
                               .Select(c => new { DisplayText = c.Name, Value = c.Id });
                    return Json(new { Result = "OK", Options = curr });
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult ClientList(int id)
        {
            try
            {
                if (User.IsInRole("Admin") || EmpRightsRepository.RightList(User.Identity.Name).Contains("R001"))
                {
                    ConsultantRepository dal = new ConsultantRepository();
                    ViewBag.consultant = new SelectList(dal.GetAll().Where(x => x.Id == id), "Id", "Name");
                    return PartialView();
                }
                else
                {
                    return PartialView("AccessDenied");
                }
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
                return PartialView("Error");
            }
        }

        [HttpPost]
        public JsonResult GetConsultantOptions()
        {
            List<ConsultantModel> model = new List<ConsultantModel>();
            model.Add(new ConsultantModel { Id = 0, Name = " " });
            var data = model.Select(c => new { DisplayText = c.Name, Value = c.Id });
            try
            {
                var list = ConsultantRepository.ConsultantOptions()
                                .Select(c => new { DisplayText = c.Name, Value = c.Id });
                return Json(new { Result = "OK", Options = list.Concat(data).OrderBy(x => x.DisplayText) });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult GetConsultants()
        {
            List<ConsultantModel> model = new List<ConsultantModel>();
            try
            {
                ConsultantRepository dal = new ConsultantRepository();
                var list = dal.GetAll()
                                .Select(c => new { DisplayText = c.Name, Value = c.Id });
                return Json(new { Result = "OK", Options = list });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult DeleteClients(ClientsModel model, int consltid)
        {
            try
            {

                if (User.IsInRole("Admin") || EmpRightsRepository.RightList(User.Identity.Name).Contains("R002"))
                {
                    ConsultantRepository.DeleteClients(consltid,model.Id);
                    return Json(new { Result = "OK", Record = model });
                }
                else
                {
                    return Json(new { Result = "Error", Message = "Sorry, You are not Authorized to do this action" });
                }
                
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

    }
}