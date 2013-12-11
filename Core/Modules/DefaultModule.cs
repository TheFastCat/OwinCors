using Nancy;

namespace Core
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