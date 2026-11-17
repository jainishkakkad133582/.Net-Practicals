using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Practical_8.Data;

namespace Practical_8
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            // Create the database and Products table on the first application run.
            DatabaseInitializer.Initialize();
        }
    }
}
