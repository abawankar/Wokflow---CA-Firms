using ApplicationServices.Web;
using DAL.Transaction;
using Domain.Interface.Transaction;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using UserInterface.Models.Master;
using UserInterface.Models.Transaction;

namespace UserInterface.Controllers.Transaction
{
    [HandleError]
    public class NBOController : ApplicationBaseController
    {
        private static log4net.ILog Log { get; set; }
        ILog log = log4net.LogManager.GetLogger(typeof(NBOController));

        public ActionResult Index()
        {
            if (User.IsInRole("Admin") || EmpRightsRepository.RightList(User.Identity.Name).Contains("R006"))
            {
                ViewBag.client = new SelectList(ClientRepository.ClientOptions().OrderBy(x=>x.Name), "Id", "Name");

                TaskTypesRepository file = new TaskTypesRepository();
                ViewBag.tasktype = new SelectList(file.GetAll().OrderBy(x=>x.Name),"Id","Name");
                return View();
            }
            else
            {
                return View("Security");
            }
        }

        [HttpPost]
        public JsonResult List(string name = null,int client=0,int tasktype=0, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                NBORepository dal = new NBORepository();
                List<NBOModel> model = new List<NBOModel>();
                if (User.IsInRole("Admin") || User.IsInRole("Accountant"))
                {
                    model = dal.GetAll().ToList();
                }
                else
                { 
                    EmployeeRepository emp = new EmployeeRepository();
                    var clientid = emp.GetById(EmployeeRepository.GetByUserName(User.Identity.Name)).Client.Select(x => x.Id).ToArray();
                    model = dal.GetAll().Where(x=>clientid.Contains(x.Client.Id)).ToList();
                }
                if(client != 0)
                {
                    model = model.Where(x => x.Client.Id == client).ToList();
                }
                if (tasktype != 0)
                {
                    model = model.Where(x => x.TaskTypes.Id == tasktype).ToList();
                }
                if(!string.IsNullOrEmpty(name))
                {
                    model = model.Where(x => x.FileNumber == Convert.ToInt32(name)).ToList();
                }
                int count = model.Count;
                model = model.OrderByDescending(x => x.Received).ToList();
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
        public JsonResult Create(NBOModel model)
        {
            try
            {
                if (User.IsInRole("Admin") || EmpRightsRepository.RightList(User.Identity.Name).Contains("R005"))
                {
                    model.UserName = User.Identity.Name;
                    NBORepository dal = new NBORepository();
                    dal.Insert(model);

                    ClientRepository c = new ClientRepository();
                    int taskid = NBODAL.GetLatestTaskId();
                    NBOModel task = dal.GetById(taskid);
                    ClientsModel client = c.GetById(model.ClientId);

                    #region --- Send mail for launching task number to client

                    MailMessage msg = new MailMessage();
                    msg.To.Add(new MailAddress(client.EmailId));
                    msg.From = new MailAddress("nsb@nimbusconsulting.in", "Task Assignment");
                    msg.CC.Add(new MailAddress(User.Identity.Name));

                    string body = "<p> Dear Sir/Mam </p> Its our plasure to work with you. Below Task number is genrated for your work assigned to us ";
                    body = body + "</p><br/><p><b>" + "Task Number: " + task.FileNumber;
                    body = body + "</b><br/><br/> Please use the same task number for all comunication with us.";
                    body = body + "</b><br/><br/> You can check your work progress on http://nimbusconsulting.in/. for username and passowrd please revert on same mail";
                    body = body + "</b></p><p>" + " Regards <br/> This is an automatic mail </p>";

                    msg.Subject = "Task number for your work: " + task.FileNumber;

                    msg.ReplyToList.Add(new MailAddress(User.Identity.Name));
                    msg.Body = body;
                    msg.IsBodyHtml = true;

                    //Controllers.EmailSetting.SendEmail(msg);

                    #endregion
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
        public JsonResult Update(NBOModel model)
        {
            try
            {
                if (User.IsInRole("Admin") || EmpRightsRepository.RightList(User.Identity.Name).Contains("R005"))
                {
                    NBORepository dal = new NBORepository();
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

        public JsonResult GetNBOOptions(int status)
        {
            try
            {
                NBORepository dal = new NBORepository();
                if(status != 0)
                {
                    var data = from m in dal.GetAll()
                               select new { Id = m.Id, FileNumber = m.FileNumber };
                    var list = data.Select(c => new { DisplayText = c.FileNumber, Value = c.Id });
                    return Json(new { Result = "OK", Options = list });
                }
                else
                {
                    var data = from m in dal.GetAll().Where (x=>x.Status ==1)
                               select new { Id = m.Id, FileNumber = m.FileNumber };
                    var list = data.Select(c => new { DisplayText = c.FileNumber, Value = c.Id });
                    return Json(new { Result = "OK", Options = list });
                }
                
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult AddDocument(int id)
        {
            ViewBag.sampId = id;
            return PartialView();
        }

        [HttpPost]
        public ActionResult UploadImage(int sampId, HttpPostedFileWrapper imageFile)
        {
            try
            {
                NBORepository dal = new NBORepository();
                int tot = dal.GetById(sampId).DocumentList.Count();

                string[] filetypestirng = imageFile.FileName.Split('.');
                string filetype = filetypestirng[filetypestirng.Length - 1];

                var fileName = String.Format("{0}." + filetype, sampId + "-" + tot);
                var imagePath = Path.Combine(Server.MapPath(Url.Content("~/Upload/Document")), fileName);
                imageFile.SaveAs(imagePath);


                NboDocumentModel bl = new NboDocumentModel();
                bl.FileName = imageFile.FileName;
                bl.FileOnServer = fileName;
                bl.FileSize = (Convert.ToDouble(imageFile.ContentLength) * 0.000000954).ToString() + "MB";
                dal.AddDocument(bl, sampId);
                ViewData["message"] = "Successfully Uploaded";
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
            }
            return RedirectToAction("Index", "NBO");
        }

        [HttpPost]
        public JsonResult DocumentList(int id)
        {
            try
            {
                NboDocumentRepository dal = new NboDocumentRepository();
                var model = dal.GetAll(id);
                int count = model.Count;
                return Json(new { Result = "OK", Records = model });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult DeleteDocument(NboDocumentModel model)
        {
            try
            {
                NboDocumentDAL dal = new NboDocumentDAL();
                INboDocument bl = dal.GetById(model.Id);
                if (dal.Delete(bl) == true)
                {
                    var imagePath = Path.Combine(Server.MapPath(Url.Content("~/Upload/Document")), bl.FileOnServer);
                    System.IO.File.Delete(imagePath);
                }

                return Json(new { Result = "OK", Record = model });
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult TaskComments(int id)
        {
            try
            {
                TaskManagerRepository dal = new TaskManagerRepository();
                var model = dal.GetTaskByFileId(id).SelectMany(x => x.Comments);
                return Json(new { Result = "OK", Records = model });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        public ActionResult Finance(int id, string file)
        {
            if (User.IsInRole("Admin") || EmpRightsRepository.RightList(User.Identity.Name).Contains("R010"))
            {
                ViewBag.nboid = id;
                ViewBag.filenumber = file;
                return PartialView();
            }
            else
            {
                return PartialView("AccessDenied");
            }
        }

    }
}