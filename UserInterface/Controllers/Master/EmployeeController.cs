using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ApplicationServices.Web;
using UserInterface.Models.Master;
using log4net;

namespace UserInterface.Controllers.Master
{      
    public class EmployeeController : ApplicationBaseController
    {
        private static log4net.ILog Log { get; set; }
        ILog log = log4net.LogManager.GetLogger(typeof(EmployeeController));

        // View Employee
        public ActionResult Index()
        {
            ViewBag.status = new SelectList(new[] { 
                new SelectListItem{Value="1", Text="Active"},
                new SelectListItem{Value="2", Text="InActive"},
                }, "Value", "Text");

            if (User.IsInRole("Admin") || EmpRightsRepository.RightList(User.Identity.Name).Contains("R004"))
            { return View(); }
            else{
                return View("Security");
            }
        }

        [HttpPost]
        public JsonResult List(string name = null,int status=0, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                EmployeeRepository dal = new EmployeeRepository();
                List<EmployeeModel> model = new List<EmployeeModel>();

                if (string.IsNullOrEmpty(name))
                {
                    model = dal.GetAll().ToList();
                }
                else
                {
                    model = dal.GetAll().Where(x=>(x.Name + " " + x.Emailid + " " + x.PAN + " " + x.MobileNo).ToLower().Contains(name.ToLower()) ).ToList();
                }
                if(status != 0)
                {
                    model = model.Where(x => x.Status == status).ToList();
                }
                int count = model.Count;
                model = model.OrderBy(x => x.Name).ToList();
                List<EmployeeModel> Model1 = model.Skip(jtStartIndex).Take(jtPageSize).ToList();
                return Json(new { Result = "OK", Records = Model1, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }
        
        [HttpPost]
        public JsonResult Create(EmployeeModel model)
        {
            try
            {
                if (User.IsInRole("Admin") || EmpRightsRepository.RightList(User.Identity.Name).Contains("R004"))
                {
                    if (!ModelState.IsValid)
                    {
                        return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                    }
                    EmployeeRepository dal = new EmployeeRepository();
                    dal.Insert(model);
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
        public JsonResult Update(EmployeeModel model)
        {
            try
            {
                if (User.IsInRole("Admin")  || EmpRightsRepository.RightList(User.Identity.Name).Contains("R004"))
                {
                    if (!ModelState.IsValid)
                    {
                        return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                    }
                    EmployeeRepository dal = new EmployeeRepository();
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
        public JsonResult GetEmployeeOptions()
        {
            try
            {
                EmployeeRepository dal = new EmployeeRepository();
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

        public ActionResult GetEmployee()
        {
            try
            {
                EmployeeRepository dal = new EmployeeRepository();
                var list = from m in dal.GetAll().OrderBy(x => x.Name)
                           select new { Id = m.Id, Name = m.Name };
                return Json(list, "employee", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult AddEmpRights(string id, string list)
        {
            EmployeeRepository.AddEmpRights(Convert.ToInt32(id), list);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult GetEmpRights(int id = 0)
        {
            EmployeeRepository dal = new EmployeeRepository();
            try
            {
                var data = dal.GetById(id).EmpRight.ToList();

                return Json(new { Result = "OK", Records = data });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult ListOfEmpRights(int id = 0, int jtStartIndex = 0, int jtPageSize = 0)
        {
            EmployeeRepository dal = new EmployeeRepository();
            EmpRightsRepository cdal = new EmpRightsRepository();
            try
            {
                var empRights = dal.GetById(id).EmpRight;
                var rightId = from c in empRights
                               select c.Id;

                var list = cdal.GetAll().Where(c => !rightId.Contains(c.Id)).ToList();

                int count = list.Count;
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
        public JsonResult GetEmpRightsOptions(int id = 0)
        {
            EmployeeRepository dal = new EmployeeRepository();
            EmpRightsRepository cdal = new EmpRightsRepository();
            try
            {
                if (id == 0)
                {
                    var curr = cdal.GetAll()
                               .Select(c => new { DisplayText = c.Description, Value = c.Id });
                    return Json(new { Result = "OK", Options = curr });
                }
                else
                {
                    var curr = dal.GetById(id).EmpRight
                               .Select(c => new { DisplayText = c.Description, Value = c.Id });
                    return Json(new { Result = "OK", Options = curr });
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        //View Employee Rights Page, Only for Admin
        public ActionResult EmpRightsList(int id)
        {
            if (User.IsInRole("Admin"))
            {
                EmployeeRepository dal = new EmployeeRepository();
                ViewBag.Employee = new SelectList(dal.GetAll().Where(x => x.Id == id), "Id", "Name");
                return PartialView();
            }
            else
            {
                return View("Security");
            }
        }

        public JsonResult DeleteRights(EmpRightsModel model, int empId)
        {
            try
            {

                if (User.IsInRole("Admin") || EmpRightsRepository.RightList(User.Identity.Name).Contains("R009"))
                {
                    EmployeeRepository.DeleteRights(empId, model.Id);
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

        public ActionResult ClientList(int id)
        {
            try
            {
                if (User.IsInRole("Admin"))
                {
                    EmployeeRepository dal = new EmployeeRepository();
                    ViewBag.emp = new SelectList(dal.GetAll().Where(x => x.Id == id), "Id", "Name");
                    return PartialView();
                }
                else
                {
                    return PartialView("AccessDenied");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return PartialView("Error");
            }
        }

        [HttpPost]
        public JsonResult ListOfClients(int id = 0, string name=null, int jtStartIndex = 0, int jtPageSize = 0)
        {
            EmployeeRepository dal = new EmployeeRepository();
            ClientRepository cdal = new ClientRepository();
            try
            {
                var clients = dal.GetAll().SelectMany(x => x.Client);
                var clientid = from c in clients
                               select c.Id;

                var list = cdal.GetAll().Where(c => !clientid.Contains(c.Id)).ToList();
                if(!string.IsNullOrEmpty(name))
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

        public ActionResult AddClients(string id, string list)
        {
            EmployeeRepository.AddClients(Convert.ToInt32(id), list);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult GetClients(int id = 0)
        {
            EmployeeRepository dal = new EmployeeRepository();
            try
            {
                var data = dal.GetById(id).Client.ToList();

                return Json(new { Result = "OK", Records = data });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public JsonResult DeleteClients(ClientsModel model, int empid)
        {
            try
            {

                if (User.IsInRole("Admin"))
                {
                    EmployeeRepository.DeleteClients(empid, model.Id);
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