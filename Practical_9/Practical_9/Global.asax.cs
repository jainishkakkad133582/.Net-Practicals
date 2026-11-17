using System;
using System.Web;

namespace Practical_9
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            // Database initialization is performed on the first page request so
            // a missing LocalDB instance can be reported in the page UI.
        }
    }
}
