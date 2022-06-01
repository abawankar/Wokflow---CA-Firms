using DAL.Master;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UserInterface.Models.Master;
using UserInterface.Models.Transaction;

namespace UserInterface.Controllers.ClientView
{
    public class ClientFileController : Controller
    {
        private static log4net.ILog Log { get; set; }
        ILog log = log4net.LogManager.GetLogger(typeof(ClientFileController));
        public ActionResult Index()
        {
            return PartialView();
        }

        [HttpPost]
        public JsonResult List(string name = null, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                NBORepository dal = new NBORepository();
                List<NBOModel> model = new List<NBOModel>();
                int[] clientid = ClientRepository.ClientList(User.Identity.Name);
                if (string.IsNullOrEmpty(name))
                {
                    foreach (var item in clientid)
	                {
                        model = model.Concat(dal.GetClientFile(item).ToList()).ToList(); ;
	                }
                }
                else
                {
                    foreach (var item in clientid)
                    {
                        model = model.Concat(dal.GetClientFile(item).ToList()).ToList();
                    }
                    
                }
                int count = model.Count;
                List<NBOModel> Model1 = model.Skip(jtStartIndex).Take(jtPageSize).ToList();
                return Json(new { Result = "OK", Records = Model1, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

    }
}