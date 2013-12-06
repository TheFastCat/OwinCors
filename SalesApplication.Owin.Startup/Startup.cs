using Microsoft.Owin;
using Owin;

namespace SalesApplication.Owin.Startup
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseNancy();
        }
    }
}
