NancyStaticContentEmbeddedResourceOnIIS
=======================================

A NancyFx OWIN web application hosted on IIS


for some reason IIS won't serve the static content files leveraging Nancy.Embedded (see CustomBootstrapper.cs within SalesApplication.csproj)

SalesApplication.Azure.Website.csproj is the application entry point; uses  Microsoft.Owin.Host.SystemWeb to host 
Nancy from within the OWIN pipeline (see SalesApplication.Owin.Startup.csproj for OWIN startup class that gets booted up).

The Website project references the Owin Startup class to use via an explicit declaration from within AssemblyInfo.cs

[assembly: OwinStartup(typeof(Startup))]

The Static content  that is served by nanacy resides within the SalesApplication.UI.csproj


The application code works as expected when deployed self-hosted on an Azure Cloud Service:

  http://sel2.us/
  
  (can reach http://sel2.us/_Nancy just fine)

however as an IIS Azure Website static, embedded content doesn't seem to be served:

  http://salesapp.us/_Nancy
  


////////// Comments:

? Is IIS fighting Nancy for static content handling? it seems as though the web.config setting isn't working:

  <appSettings>
    <add key="owin:HandleAllRequests" value="true" />
  </appSettings>
