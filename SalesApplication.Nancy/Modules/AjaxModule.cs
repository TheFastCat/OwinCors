using Nancy;

namespace SalesApplication.Nancy.Modules
{
    /// <summary>
    /// This NancyModule serves as a proof of concept for serving AJAX-enabled static files
    /// </summary>
    public class AjaxModule : NancyModule
    {
        public readonly string HELLO_WORLD = "Hello AJAX!";

        public AjaxModule()
        {

            Get["/Ajax"] = _ =>
            {
                return Negotiate
                    .WithView("ajax");
            };
        }
    }
}