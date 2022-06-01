using DAL.Master;
using DAL.Transaction;
using Domain.Implementation.Master;
using Domain.Implementation.Transaction;
    using Domain.Interface.Master;
    using Domain.Interface.Transaction;
    using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using UserInterface.Models.Master;
using UserInterface.Models.Transaction;
using ApplicationServices.Web;
using log4net;

namespace UserInterface.Controllers.Transaction
{
    [HandleError]
    public class NewTaskController : ApplicationBaseController
    {
        private static log4net.ILog Log { get; set; }
        ILog log = log4net.LogManager.GetLogger(typeof(NewTaskController));

        // GET: NewTask
        public ActionResult Index()
        {
            try
            {
                if (User.IsInRole("Admin") ||  EmpRightsRepository.RightList(User.Identity.Name).Contains("R003"))
                {
                    ViewBag.today = DateTime.Today.ToString("yyyy-MM-dd");

                    ViewBag.type = new SelectList(new[] { 
                                   new SelectListItem{Value="E", Text="Employee"},
                                   new SelectListItem{Value="C", Text="Consultant"},
                                   }, "Value", "Text");
                    
                    //EmployeeDAL edal = new EmployeeDAL();
                    //var emp = from e in edal.GetAll()
                    //          select new { Id = e.Id, Name = e.Name };
                    //ViewBag.emp = new SelectList(emp,"Id","Name");
                  
                    ViewBag.conslt = new SelectList(ConsultantRepository.ConsultantOptions().OrderBy(x=>x.Name), "Id", "Name");

                    ViewBag.client = new SelectList(ClientRepository.ClientOptions().OrderBy(x=>x.Name), "Id", "Name");

                    NBORepository dal = new NBORepository();
                    var job = from m in dal.GetAll()
                              select new { Id = m.Id, Job = m.FileNumber };
                    ViewBag.job = new SelectList(job, "Id", "Job");

                    return View();
                }
                else
                {
                    return View("Security");
                }
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
                return View("Error");
            }
        }

        [HttpPost]
        public JsonResult List(string name = null, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                TaskManagerRepository dal = new TaskManagerRepository();
                List<TaskManagerModel> model = new List<TaskManagerModel>();
                if (User.IsInRole("User"))
                {
                    model = dal.GetTodayTask(EmployeeRepository.GetByUserName(User.Identity.Name)).ToList();
                    model = model.Concat(dal.GetByAssignee(EmployeeRepository.GetByUserName(User.Identity.Name))).ToList();
                    
                }
                if (User.IsInRole("Admin"))
                {
                    model = dal.GetAll().ToList();
                }
                int count = model.Count;
                List<TaskManagerModel> Model1 = model.Skip(jtStartIndex).Take(jtPageSize).ToList();
                return Json(new { Result = "OK", Records = Model1, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }
        [HttpPost]
        public ActionResult CreateTask(TaskManagerModel obj)
        {
            try
            {
                string mailid = "";
                EmployeeRepository e = new EmployeeRepository();
                IEmployee employee = e.GetAll().Where(x => x.Emailid == User.Identity.Name).Take(1).SingleOrDefault();

                obj.Date = System.DateTime.Today;
                TaskManagerDAL dal = new TaskManagerDAL();
                ITaskManager bl = new TaskManager();
                bl.Date = obj.Date;
                bl.ContactType = obj.Type;

                if (obj.Type == "C")//Task assigned to consultant
                {
                    IConsultant cnslt = new Consultant { Id = obj.ConsltId };
                    bl.Consultant = cnslt;
                }
                bl.Task = obj.Task;
                bl.Notes = obj.Notes;
                bl.Status = 1;
                bl.StatusPercentage = 0;
                bl.Start = Convert.ToDateTime(obj.Start.ToString("yyyy-MM-dd"));// +DateTime.Now.TimeOfDay;
                bl.Due = Convert.ToDateTime(obj.Start.ToString("yyyy-MM-dd")) + DateTime.Now.TimeOfDay;

                INBO file = new NBO { Id = obj.NboId };
                bl.FileNumber = file;

                //bl.Compl = obj.Compl != null ? Convert.ToDateTime((Convert.ToDateTime(obj.Compl).ToString("yyyy-MM-dd"))) + DateTime.Now.TimeOfDay : obj.Compl;
                IEmployee emp = new Employee { Id = employee.Id };
                bl.Assigneer = emp;
                string toName = "";
                bool isPartner = false;
                if (!string.IsNullOrEmpty(obj.Assignerlist))// Task assigned to Partner / EMployee
                {
                    EmployeeDAL edal = new EmployeeDAL();
                    string[] assign = obj.Assignerlist.Split(',');
                    List<IEmployee> emplist = new List<IEmployee>();
                    foreach (var item in assign)
                    {
                        IEmployee b = edal.GetById(Convert.ToInt32(item));
                        mailid = mailid + "," + b.Emailid;
                        if (b.Position.Name == "Partner")
                            isPartner = true;
                             
                        if (toName == "")
                        {
                            toName = b.Name;
                        }
                        else
                        {
                            toName = toName + "/" + b.Name;
                        }
                        emplist.Add(b);
                    }
                    bl.Contacts = emplist;

                }
                dal.InsertOrUpdate(bl);

                NBORepository task = new NBORepository();
                NBOModel taskid = task.GetById(obj.NboId);

                if (!string.IsNullOrEmpty(obj.Assignerlist))
                {
                    string subject = null;
                    string body = null;

                    string clientSubject = null;
                    string clientBody = null;

                    if(isPartner == true)
                    {
                        subject = "Client :" + taskid.Client.Name + " has been assigned to you.";
                        body = "<p> Dear " + toName + "</p> Kindly note that the task " + taskid.FileNumber +
                        " for client " + taskid.Client.Name + " has been assigned to you.";
                        body = body + "</b></p><p>" + " Regards <br/> This is an automatic mail </p>";

                        clientSubject = "Partner in charge of your task for " + taskid.TaskTypes.Name;
                        clientBody = "<p> Dear Sir/Mam </p><p> Ms/Mr " + employee.Name + " is partner in charge of your task for " + taskid.TaskTypes.Name;
                        clientBody = clientBody + "<p>You can see the progress of your work by login with your user id and password.";
                        clientBody = clientBody + "</p><p>" + " Regards <br/> This is an automatic mail </p>";
                    }
                    else
                    {
                        subject = "New Task Assignment: Task number: " + taskid.FileNumber + " Client: " + taskid.Client.Name;
                        body = "<p> Dear " + toName + "</p> You have been assigned task no " + taskid.FileNumber + " by " +
                            employee.Name + " to be completed by " + Convert.ToDateTime(bl.Due).ToString("dd MMM yyyy");
                        body = body + "</p><br/><p>Please keep your task updated in the portal by login using your id and password</p> ";
                        body = body + "</b><p>" + " Regards <br/> This is an automatic mail </p>";

                        clientSubject = "Partner in charge of your task for " + taskid.TaskTypes.Name;
                        clientBody = "<p> Dear Sir/Mam </p><p> Partner in charge of your task " + taskid.TaskTypes.Name + " has assigned field/ desk work to " + toName;
                        clientBody = clientBody + " Your can see the progress of your work by login with your user id and password.";
                        clientBody = clientBody + "</b></p><p>" + " Regards <br/> This is an automatic mail </p>";
                    }
                    #region  -- sending mail to assigner and client-----

                    MailMessage msg = new MailMessage();
                    string[] mail = mailid.Split(',');
                    for (int i = 1; i < mail.Length; i++)
                    {
                        msg.To.Add(new MailAddress(mail[i].ToString()));
                    }
                    //msg.CC.Add(new MailAddress(User.Identity.Name));
                    msg.ReplyToList.Add(new MailAddress(User.Identity.Name));
                    msg.Subject = subject;
                    msg.Body = body;
                    msg.IsBodyHtml = true;

                    MailMessage msgCLient = new MailMessage();
                    msgCLient.To.Add(new MailAddress(taskid.Client.EmailId));
                    //msgCLient.CC.Add(new MailAddress(User.Identity.Name));
                    msgCLient.ReplyToList.Add(new MailAddress(User.Identity.Name));
                    msgCLient.Subject = clientSubject;
                    msgCLient.Body = clientBody;
                    msgCLient.IsBodyHtml = true;

                    try
                    {
                        EmailSetting.SendEmail(msg);
                        TempData["message"] = "Task Assigned Successfulyy";
                        EmailSetting.SendEmail(msgCLient);
                        TempData["mail"] = "Mail sent to Client";
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                    #endregion
                    
                }

                if(obj.Type == "C")
                {
                    //ConsultantRepository consltdal = new ConsultantRepository();
                    //ConsultantModel conslModel = consltdal.GetById(obj.ConsltId);

                }

                //EmailSetting.SMSSendToLenders(Convert.ToInt64(task.Client.MobileNo), message);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return View("Error");
            }
        }

        #region -- Mail setting --

        public static void SendEmail(MailMessage m)
        {
            SendEmail(m, true);
        }

        public static void SendEmail(MailMessage m, Boolean Async)
        {
            SmtpClient smtpClient = null;
            smtpClient = new SmtpClient();
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new System.Net.NetworkCredential("info@designedsolutions4u.in", "Jeza6h#Wraq");
            smtpClient.Port = 25;
            smtpClient.Host = "mail.designedsolutions4u.in";
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.EnableSsl = false;
            if (Async)
            {
                SendEmailDelegate sd = new SendEmailDelegate(smtpClient.Send);
                AsyncCallback cb = new AsyncCallback(SendEmailResponse);
                sd.BeginInvoke(m, cb, sd);
            }
            else
            {
                smtpClient.Send(m);
            }
        }

        private delegate void SendEmailDelegate(MailMessage m);

        private static void SendEmailResponse(IAsyncResult ar)
        {
            SendEmailDelegate sd = (SendEmailDelegate)(ar.AsyncState);
            sd.EndInvoke(ar);
        }

        #endregion

    }
}