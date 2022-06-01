using System.Data.Common;
using System.Web;
using System.Web.Mvc;

namespace UserInterface
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute
            {
                ExceptionType = typeof(DbException),
                View = "DbError",
                Order = 2
            });
            filters.Add(new HandleErrorAttribute());
        }
    }
}
