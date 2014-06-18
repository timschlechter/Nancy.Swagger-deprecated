using Nancy.Bootstrapper;
using Nancy.Conventions;
using Nancy.Responses.Negotiation;

namespace Nancy.Swagger.TestApp
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override void ApplicationStartup(TinyIoc.TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);

            
            Nancy.Swagger.StaticConfiguration.ModulePath = "/api";

            pipelines.AfterRequest.AddItemToEndOfPipeline(context =>
            {
                context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            });
        }

        protected override Nancy.Bootstrapper.NancyInternalConfiguration InternalConfiguration
        {
            get
            {
                return NancyInternalConfiguration.WithOverrides((c) =>
                {
                    c.ResponseProcessors.Remove(typeof(ViewProcessor));
                    c.ResponseProcessors.Remove(typeof(XmlProcessor));
                });
            }
        }
    }
}