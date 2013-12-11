OwinCors
=======================================

This stub project is useful for testing OWIN CORs enablement on both OWIN SystemWeb host (Website .csproj) and self-hosted via httplistener (cloud service worker role .csproj)

To repro:

	F5 the website project (SystemWeb OWIN hosting) 
	Use an extension against this URL: http://localhost:4444/hellonancy
		to investigate the header composition

Expected: to see the following in the header contents:
	Access-Control-Allow-Methods →GET, POST, OPTIONS, PUT, DELETE
	Access-Control-Allow-Origin →*

result: can see following in the header contents:
	Access-Control-Allow-Methods →GET, POST, OPTIONS, PUT, DELETE
	Access-Control-Allow-Origin →*


Now try the same thing from the Azure.CloudService.csproj (which executes the Azure.CloudService.Roles.Owin.csproj to invoke the OWIN pipeline):

	F5 the Azure.CloudService project (HttpListener OWIN hosting) 
	Use an extension against this URL: http://127.0.0.1:4444/hellonancy  (endpoint of emulated cloud service)
		to investigate the header composition

Expected: to see the following in the header contents:
	Access-Control-Allow-Methods →GET, POST, OPTIONS, PUT, DELETE
	Access-Control-Allow-Origin →*

result: can see following in the header contents:
	No CORs-specific headers present


