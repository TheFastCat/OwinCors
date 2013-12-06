using System;
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Diagnostics;
using System.Diagnostics;
using System.Web.Http;
using System.Web.Http.Dependencies;
using System.Web.Http.Dispatcher;
using System.Net;
using Nancy;
using Nancy.Owin;
using SalesApplication.Nancy.Environment;

namespace SalesApplication.Owin
{
    // Summary: OWIN extension methods for SalesApplication
    public static class Extensions
    {
        /// <summary>
        /// Display a nicely-formatted error page when errors occur, but only in DEBUG mode
        /// </summary>
        [Conditional("DEBUG")]
        public static void UseErrorPageInDebugMode(this IAppBuilder app)
        {
            ErrorPageOptions options = new ErrorPageOptions();
            options.SetDefaultVisibility(true);
            app.UseErrorPage(options);
        }

        /// <summary>
        /// Display a nicely-formatted Welcome page as default view, but only in DEBUG mode
        /// </summary>
        [Conditional("DEBUG")]
        public static void UseWelcomePageInDebugMode(this IAppBuilder app, string pathString = "")
        {
             app.UseWelcomePage(pathString);
        }

        /// <summary>
        /// Facade to ASP.NET Web API OWIN startup; bundles configuration
        /// </summary>
        /// <see cref="http://is.gd/wYhSQQ"/>
        public static void UseSalesApplicationWebAPI(this IAppBuilder app)
        {
            var config = new HttpConfiguration();
            // TODO: decouple Ninject from this SalesApplication.Owin.csproj into a separate assembly
            config.Routes.MapHttpRoute("customers", "api/{Controller}");
            config.Routes.MapHttpRoute("topAcctTypes", "api/{Controller}");
            config.Formatters.XmlFormatter.SupportedMediaTypes.Clear();//toggles JSON as default formatter...
            app.UseWebApi(config);
        }

        /// <summary>
        /// Enable NTLM authentication for the HTTP listener host
        /// </summary>
        /// <param name="app"></param>
        public static void UseNTLM(this IAppBuilder app)
        {
            var listener = (HttpListener)app.Properties["System.Net.HttpListener"];
            listener.AuthenticationSchemes = AuthenticationSchemes.Ntlm;
        }

        /// <summary>
        /// Configure Nancy's custom startup configuration
        /// </summary>
        /// <remarks>Reid: I wish this wasn't so kludgey - but I don't know how to more elegantly 
        /// solve the case I outline here: http://goo.gl/GJoJfE </remarks>
        public static void UseNancy(this IAppBuilder app)
        {
            // A little note about the following from Reid:
            // 
            //      One of the requirements I made for our architecture is that all things relating to
            //  "UI" -- assets, images, scripts etc -- should reside in a centralized assembly as embedded
            //  resources within that assembly. I took this a step further in order to associate web browser
            //  favicons with UI elements. So now favicons used by both the Website and CloudService apps
            //  reside within the SalesApplication.UI.csproj as embedded resources and Nancy needs to figure out
            //  which favicon to load (read:which Bootstrapper) at runtime based on which application is using her (Nancy).
            //  it seems a little overengineered and somewhat kludgey but maintains segreation of UI assets.
            
            NancyOptions nancyOptions = new NancyOptions();

            // retrieve the name of the executing application from OWIN
            // from this we will determine the execution context and which 
            // bootstrapper to load
            string appName = app.Properties["host.AppName"].ToString();

            if (appName.Contains("SalesApplication.Azure.Website"))
            {
                // the INancyBootstrapper used for the Website
                // currently this just defines a custom favicon to use for the Website
                // to distinguish it from the CloudService at runtime
                nancyOptions.Bootstrapper = new WebsiteBootstrapper();
            }
            else // assume we're running from the Cloud Service
            {
                // the INancyBootstrapper used for the CloudService
                // currently this just defines a custom favicon to use for the CloudService
                // to distinguish it from the Website at runtime
                nancyOptions.Bootstrapper = new CloudServiceBootstrapper();
            }

            app.UseNancy(nancyOptions);
        }
    }
}
