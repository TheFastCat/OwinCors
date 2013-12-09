using Microsoft.Owin;
using Owin;
using Nancy.Owin;
using Microsoft.Owin.Extensions;

namespace Startup
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseNancy();
            //app.UseStageMarker(PipelineStage.MapHandler);
        }
    }
}
