using Microsoft.Owin.Hosting;
using Microsoft.WindowsAzure.ServiceRuntime;
using Ninject;
using SalesApplication.Owin.Startup;
using System;
using System.Diagnostics;
using System.Net;
using System.Threading;

namespace SalesApplication.Azure.CloudService.Roles.Owin
{
    public class RoleStartup : RoleEntryPoint
    {
        private IDisposable MyApp = null;
        private IKernel Kernel { get; set; }

        public override void Run()
        {
            // this is a sample worker implementation. Replace with your logic.
            Trace.TraceInformation("WorkerRole1 entry point called", "Information");

            while (true)
            {
                Thread.Sleep(10000);
                Trace.TraceInformation("Working", "Information");
            }
        }

        public override bool OnStart()
        {
            ServicePointManager.DefaultConnectionLimit = 50;
            var AppPoint = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["OwinEndpoint"];
            string baseUri = string.Format("{0}://{1}", AppPoint.Protocol, AppPoint.IPEndpoint);
            Trace.TraceInformation(String.Format("OWIN URL is {0}", baseUri), "Information");

            // initialize OWIN pipeline
            // we explicitly invoke our OWIN Startup class instead of configuring it or allowing
            // discovery
            MyApp = WebApp.Start<Startup>(new StartOptions(url: baseUri));

            return base.OnStart();
        }

        public override void OnStop()
        {
            if (MyApp != null)
            {
                MyApp.Dispose();
            }
            base.OnStop();
        }
    }
}
