using Microsoft.Owin;
using Owin;
using Nancy.Owin;
using Microsoft.Owin.Extensions;
//using Microsoft.Owin.Cors;
using Simple.Owin.CorsMiddleware;

namespace Startup
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //app.UseCors(CorsOptions.AllowAll);
            Simple.Owin.Cors.Create(OriginMatcher.Wildcard).Build();
            app.UseNancy();
            app.UseStageMarker(PipelineStage.MapHandler);
        }
    }
}
