using Nancy.Bootstrapper;

namespace Nancy.Swagger.TestApp
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override Nancy.Bootstrapper.NancyInternalConfiguration InternalConfiguration
        {
            get
            {
                return NancyInternalConfiguration.WithOverrides((c) =>
                {
                });
            }
        }

        protected override void ApplicationStartup(TinyIoc.TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);

            pipelines.AfterRequest.AddItemToEndOfPipeline(context =>
            {
                context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            });
        }
    }
}