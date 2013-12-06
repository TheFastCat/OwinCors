using Nancy;

namespace SalesApplication.Nancy.Modules
{
    public class DefaultModule : NancyModule
    {
        public readonly string HELLO_WORLD = "Hello World!";

        public DefaultModule()
        {
            Get["/hellonancy"] = _ => HELLO_WORLD;
        }
    }
}