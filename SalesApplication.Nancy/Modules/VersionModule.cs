using System;
using System.Linq;
using Nancy;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using SalesApplication.Info;
using SalesApplication.Info.Model;

namespace SalesApplication.Nancy.Modules
{
    /// <summary>NancyModule containing route definitions to retrieve application versioning information</summary>
    public class VersionModule : NancyModule
    {
        public VersionModule(IApplicationInfo applicationInfo)
        {
            IApplicationInfo _applicationInfo = applicationInfo;

            Get["/versions/all"] = _ =>
            {
                IEnumerable<AssemblyData> appDomainAssemblyInformation = _applicationInfo.GetCurrentAppDomainAssemblyInformation();

                // bind the Queue to the versions view to output its display
                return Negotiate
                    .WithView("versions")
                    .WithModel(appDomainAssemblyInformation);
            };
            Get["/versions"] = _ =>
            {
                IEnumerable<AssemblyData> appDomainAssemblyInformation = _applicationInfo.GetCurrentAppDomainAssemblyInformation("SalesApplication");

                // bind the Queue to the versions view to output its display
                return Negotiate
                    .WithView("versions")
                    .WithModel(appDomainAssemblyInformation);
            };
            Get["/versions/{nameFilter}"] = parameters =>
            {
                IEnumerable<AssemblyData> appDomainAssemblyInformation = _applicationInfo.GetCurrentAppDomainAssemblyInformation(parameters.nameFilter);

                // bind the Queue to the versions view to output its display
                return Negotiate
                    .WithView("versions")
                    .WithModel(appDomainAssemblyInformation);
            };
        }
    }
}
