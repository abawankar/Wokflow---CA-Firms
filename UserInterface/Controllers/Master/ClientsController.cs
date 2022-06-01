using ApplicationServices.Web;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UserInterface.Models.Master;
using UserInterface.Models.Transaction;

namespace UserInterface.Controllers.Master
{
    [HandleError]
    public class ClientsController : ApplicationBaseController
    {
        private static log4net.ILog Log { get; set; }
        ILog log = log4net.LogManager.GetLogger(typeof(ClientsController));

        // GET: Clients
        public ActionResult Index()
        {
            if (User.IsInRole("Admin") || EmpRightsRepository.RightList(User.Identity.Name).Contains("R007"))
            { 
                return View(); 
            }
            else
            {
                return View("Security");
            }
        }

        [HttpPost]
        public JsonResult List(string name = null, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                ClientRepository dal = new ClientRepository();
                List<ClientsModel> model = new List<ClientsModel>();
                if (string.IsNullOrEmpty(name))
                {
                    model = dal.GetAll().ToList();
                }
                else
                {
                    model = dal.GetAll().Where(x => (x.Name + " " +x.EmailId + " " + x.PAN + " " + x.BankAccountNumber)
                        .ToLower().Contains(name.ToLower())).ToList();
                }
                int count = model.Count;
                switch (jtSorting)
                {
                    case "Name ASC":
                        model = model.OrderBy(x => x.Name).ToList();
                        break;
                    case "Name DSC":
                        model = model.OrderByDescending(x => x.Name).ToList();
                        break;
                }
                List<ClientsModel> Model1 = model.Skip(jtStartIndex).Take(jtPageSize).ToList();
                return Json(new { Result = "OK", Records = Model1, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Create(ClientsModel model)
        {
            try
            {
                if (User.IsInRole("Admin") || EmpRightsRepository.RightList(User.Identity.Name).Contains("R008"))
                {
                    if (!ModelState.IsValid)
                    {
                        return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                    }
                    ClientRepository dal = new ClientRepository();
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
                Log.Error(ex.Message);
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Update(ClientsModel model)
        {
            try
            {
                if (User.IsInRole("Admin") || EmpRightsRepository.RightList(User.Identity.Name).Contains("R008"))
                {
                    if (!ModelState.IsValid)
                    {
                        return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                    }
                    ClientRepository dal = new ClientRepository();
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
                Log.Error(ex.Message);
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult GetClientsOptions()
        {
            List<ClientsModel> model = new List<ClientsModel>();
            model.Add(new ClientsModel { Id = 0, Name = "* Select *" });
            var data = model.Select(c => new { DisplayText = c.Name, Value = c.Id });
            try
            {
                ClientRepository dal = new ClientRepository();
                var list = ClientRepository.ClientOptions().OrderBy(X => X.Name)
                                .Select(c => new { DisplayText = c.Name, Value = c.Id });
                return Json(new { Result = "OK", Options = list.Concat(data).OrderBy(x => x.DisplayText) });
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

            
        }

        [HttpPost]
        public JsonResult ContactList(int clientid)
        {
            try
            {
                ClientContactRepository dal = new ClientContactRepository();
                var model = dal.GetAll(clientid);
                int count = model.Count;
                return Json(new { Result = "OK", Records = model });
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult UpdateContact(ClientContactModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }
                ClientContactRepository dal = new ClientContactRepository();
                dal.Edit(model);
                return Json(new { Result = "OK", Record = model });
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult AddContact(ClientContactModel model, int clientid)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }
                ClientRepository dal = new ClientRepository();
                dal.AddContact(model, clientid);
                return Json(new { Result = "OK", Record = model });
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult GetConsultantClients(int cid)
        {
            try
            {
                if (cid != 0)
                {
                    ConsultantRepository dal = new ConsultantRepository();
                    var model = from m in dal.GetById(cid).Clients
                                select new { Id = m.Id, Name = m.Name };
                    return Json(new { Result = model.OrderBy(x => x.Name) });
                }
                else
                {
                    ClientRepository dal = new ClientRepository();
                    var model = from m in dal.GetAll()
                                select new { Id = m.Id, Name = m.Name };

                    return Json(new { Result = model.OrderBy(x => x.Name) });
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return Json(new { Result = "Error", Message = ex.Message });
            }
            
        }

        public JsonResult GetEmployeeClients(string cid)
        {
            try
            {
                if (!string.IsNullOrEmpty(cid))
                {
                    EmployeeRepository dal = new EmployeeRepository();
                    var model = from m in dal.GetAll().Where(x=>cid.Contains(x.Id.ToString())).SelectMany(y=>y.Client)
                                select new { Id = m.Id, Name = m.Name };
                    return Json(new { Result = model.OrderBy(x => x.Name) });
                }
                else
                {
                    ClientRepository dal = new ClientRepository();
                    var model = from m in dal.GetAll()
                                select new { Id = m.Id, Name = m.Name };

                    return Json(new { Result = model.OrderBy(x => x.Name) });
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return Json(new { Result = "Error", Message = ex.Message });
            }

        }

        [HttpPost]
        public JsonResult GetClientsFile(int cid)
        {
            try
            {
                NBORepository dal = new NBORepository();
                if (cid != 0)
                {
                    var model = from m in dal.GetAll().Where(x => x.Client.Id == cid)
                                select new { Id = m.Id, Name = m.FileNumber };
                    return Json(new { Result = model.OrderBy(x => x.Name) });
                }
                else
                {
                    var model = from m in dal.GetAll()
                                select new { Id = m.Id, Name = m.FileNumber };

                    return Json(new { Result = model.OrderBy(x => x.Name) });
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return Json(new { Result = "Error", Message = ex.Message });
            }

        }

    }
}