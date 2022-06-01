using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UserInterface.Controllers.Admin
{
    public class AccessDeniedController : ApplicationBaseController
    {
        // GET: AccessDenied
        public ActionResult Security()
        {
            return View();
        }
    }
}