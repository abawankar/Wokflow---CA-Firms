using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UserInterface.Controllers.Admin
{
    public class ErrorController : ApplicationBaseController
    {
        // GET: Error
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult HttpError()
        {
            return View("Error");
        }

        public ActionResult NotFound()
        {
            return View();
        }

        public ActionResult Unauthorized()
        {
            return View();
        }
    }
}