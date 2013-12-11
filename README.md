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


note: remove the following from web.config to break this functionality:

    <httpProtocol>
      <customHeaders>
        <add name="Access-Control-Allow-Origin" value="*" />
        <add name="Access-Control-Allow-Methods" value="GET, POST, OPTIONS, PUT, DELETE" />
      </customHeaders>
    </httpProtocol>
    
