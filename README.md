NancyIISHostingNotWorking
=======================================

for some reason my SystemWeb hosting of OWIN & Nancy is not functioning

I created a demo repo for repro: https://github.com/TheFastCat/NancyIISHostingNotWorking

To repro:

	F5 the website project (SystemWeb OWIN hosting) and navigate to /_Nancy

Expected:
	Static assets for the diagnostics page

result:
	Skeleton Nancy Diagnostics page 

- no diagnostics assets

try the same thing with the Azure.CloudService.csproj (self hosted) and it functions as expected.

both projects leverage and invoke the same OWIN pipeline that boots up Nancy
in both projects the /HelloNancy route is handled as expected


////////// Comments:

? Is IIS fighting Nancy for static content handling? it seems as though the web.config setting isn't working:

  <appSettings>
    <add key="owin:HandleAllRequests" value="true" />
  </appSettings>
