using Microsoft.Owin;
using Owin;
using Nancy.Owin;
using Microsoft.Owin.Extensions;
using Microsoft.Owin.Cors;

namespace Startup
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);
            app.UseNancy();
            app.UseStageMarker(PipelineStage.MapHandler);
        }
    }
}
