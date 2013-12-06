  //////////////////////////////////////////////////////////////
 //// SalesApplication.Owin OVERVIEW         version 0.0.3 ////
//////////////////////////////////////////////////////////////

	This .csproj contains OWIN extension methods etc to be invoked via SalesApplication.Owin.Startup.Startup class.
The code within the extension methods could all be placed within the OWIN startup pipeline however it was moved here
and implemented as extension methods in order to simplify and keep the OWIN pipeline clean.

This project will also contain any custom OWIN Middleware that we create to execute within the OWIN pipeline.
An example of useful OWIN middleware: http://www.strathweb.com/?p=988 and for security: http://wp.me/p1BjCh-il