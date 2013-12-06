using Nancy;
using Nancy.Bootstrapper;
using Nancy.Conventions;
using Nancy.Embedded;
using Nancy.Embedded.Conventions;
using Nancy.TinyIoc;
using Nancy.Diagnostics;
using SalesApplication.Info;
using SalesApplication.Nancy.Modules;
using SalesApplication.UI;
using System.Reflection;

namespace SalesApplication.Nancy.Environment
{
    /// <summary>
    /// See NancyFx's documentation http://goo.gl/HeXsp
    /// </summary>
    public class CustomBootstrapper : DefaultNancyBootstrapper
    {
        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            // enable for NancyFx diagnostics
            StaticConfiguration.EnableRequestTracing = true;

            // register the dependencies for our module classes...
            container.Register<IApplicationInfo,    ApplicationInfo>();

            // see http://is.gd/yezLsQ for embedding views into Nancy's assemblies
            // to make consumption by client assemblies easier
            // this is the assembly your views are embedded in
            Assembly viewsAssembly = GetUIAssembly();
             
            // point to the Views directory containing our views;
            // generally, in Visual Studio, this will be "YourAssemblyName.PathToViewDirectory" 
            EmbeddedViewLocationProvider.RootNamespaces.Add(viewsAssembly, "SalesApplication.UI.Views");
            
            base.ConfigureApplicationContainer(container);
        }

        protected virtual Assembly GetUIAssembly()
        {
            // see the code documentation for the SalesApplication.UI.Hooker.cs class
            // for context into this invocation...
            Assembly viewAssembly = typeof(Hooker).Assembly;
            return viewAssembly;
        }

        protected override NancyInternalConfiguration InternalConfiguration
        {
            get
            {
                return NancyInternalConfiguration.WithOverrides(OnConfigurationBuilder);
            }
        }

        void OnConfigurationBuilder(NancyInternalConfiguration x)
        {
            // we have chosen to keep our static html/cshtml/sshtml views
            // in an assembly separate from Nancy (this is sort of uncommon)
            // because of this we override the default mechanism that Nancy
            // uses to locate views and tell Nancy to locate views that are
            // embedded as resources within an assembly. Up above we further
            // define the SalesApplication.UI assembly as the container for
            // these embedded resource views.
            x.ViewLocationProvider = typeof(EmbeddedViewLocationProvider);
        }

        /// <summary>
        /// Tells Nancy where to route requests to "Scripts" directory
        /// </summary>
        /// <remarks>This was a bitch to figure out</remarks>
        protected override void ConfigureConventions(NancyConventions nancyConventions)
        {
            base.ConfigureConventions(nancyConventions);
            Assembly uiAssembly = GetUIAssembly();

            // load static files from the following directories
            nancyConventions.StaticContentsConventions.AddDirectory("Scripts", uiAssembly, "Scripts");
            nancyConventions.StaticContentsConventions.AddDirectory("css",     uiAssembly, "css");
            nancyConventions.StaticContentsConventions.AddDirectory("Images",  uiAssembly, "Images");
            nancyConventions.StaticContentsConventions.AddDirectory("Fonts",   uiAssembly, "Fonts");
        }

        /// <summary>
        /// Configures a password for the NancyFx diagnostics page useful for debugging.
        /// Nancy's diagnostics can be reached via http://<address-of-your-application>/_Nancy/
        /// Login password is configured below
        /// </summary>
        /// <see cref="https://github.com/NancyFx/Nancy/wiki/Diagnostics"/>
        protected override DiagnosticsConfiguration DiagnosticsConfiguration
        {
            get { return new DiagnosticsConfiguration { Password = @"Artery22" }; }
        }
    }
}
