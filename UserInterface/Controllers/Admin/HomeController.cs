using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UserInterface.Helpers;

namespace UserInterface.Controllers
{
    public class HomeController : ApplicationBaseController
    {
        private static log4net.ILog Log { get; set; }
        ILog log = log4net.LogManager.GetLogger(typeof(HomeController));

        [Authorize]
        public ActionResult Index()
        {
            log.Info("Log Successfull");
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return PartialView();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [Authorize]
        public ActionResult Chat()
        {
            try
            {
                ViewBag.Token = MD5Helper.GetMd5Hash(TokenHelper.GetToken(User.Identity.Name));
            }
            catch(Exception ex)
            {
                log.Error("Chat Action In Home " + ex.Message);
            }
            
            return View();
        }

       
    }
}