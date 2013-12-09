  ///////////////////////////////////////////////
 //// OVERVIEW               version 0.0.02 ////
///////////////////////////////////////////////

	This .csproj is basically a "husk" that hosts the OWIN pipeline.
	The only thing it does is specify some WCF configuration options, OWIN hosting mechanism and then start up
	the OWIN pipeline; application functionality
	resides within the various SalesApplication .csprojs that this project references.

	The OWIN hosting mechanism, and OWIN pipeline is started via explicit invocation in RoleStartup.cs 

	   ////////////////
	  //Configuration/
	 ////////////////

	-The required WCF endpoint configurations by SalesApplication.Adapter.Legacy.Wcf are also defined within web.config.
	 These settings are currently required to consume legacy's WCF pipe

	   /////////////
	  //Deployment/
	 /////////////

	This website is currently deployed to Windows Azure Cloud Service and is publicly accessible via http://sel2.us/
	The deployed version probably doesn't match the code base though since there isn't a continuous integration/deployment pipeline in place 
	(yet)

	   ////////////////////////
	  //Continuous Deployment/
	 ////////////////////////

	 TODO!
