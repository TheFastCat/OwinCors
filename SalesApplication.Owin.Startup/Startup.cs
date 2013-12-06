using Microsoft.Owin;
using Owin;
using SalesApplication.Owin;
using SalesApplication.Owin.Startup;

// This attribute would explicitly direct OWIN's entrypoint for the application
// however (a) OWIN already dynamically discovers entrypoints based on naming 
// conventions so it will find our Startup without it (http://tinyurl.com/lmr6gcs) 
// (b) we explicitly direct consuming assemblies to use this startup class to avoid
// mysterious runtime behavior.
//[assembly: OwinStartup(typeof(Startup))]

namespace SalesApplication.Owin.Startup
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseWelcomePageInDebugMode("/");
            app.UseErrorPageInDebugMode();
            app.UseSalesApplicationWebAPI();
            app.UseNancy();
        }
    }
}
