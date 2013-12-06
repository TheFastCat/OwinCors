using System;
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Diagnostics;
using System.Diagnostics;
using System.Web.Http;
using System.Web.Http.Dependencies;
using System.Web.Http.Dispatcher;
using System.Net;

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
    }
}
