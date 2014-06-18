using Nancy.IO;
using Newtonsoft.Json;
using System.IO;
using System.Linq;

namespace Nancy.Swagger
{
    public class SwaggerModule : NancyModule
    {
        private NancyApiDiscoverer _discoverer = new NancyApiDiscoverer();

        #region Static Helpers

        private Response CreateStreamedJsonResponse(dynamic model)
        {
            return new Response
            {
                ContentType = "application/json",
                StatusCode = HttpStatusCode.OK,
                Contents = stream =>
                {
                    // Serialize the model to the stream
                    using (var streamWrapper = new UnclosableStreamWrapper(stream))
                    {
                        using (var streamWriter = new StreamWriter(streamWrapper))
                        {
                            using (var jsonWriter = new JsonTextWriter(streamWriter))
                            {
                                var settings = new JsonSerializerSettings
                                {
                                    NullValueHandling = NullValueHandling.Ignore
                                };
                                var serializer = JsonSerializer.Create(settings);
                                serializer.Serialize(jsonWriter, model);
                            }
                        }
                    }
                }
            };
        }

        #endregion Static Helpers

        #region Constructors

        public SwaggerModule(TinyIoc.TinyIoCContainer container)
            : base(StaticConfiguration.ModulePath)
        {
            var types = _discoverer.GetModuleTypesToDocument();
            var modules = types.Select(t => container.Resolve(t) as NancyModule);

            var factory = new SwaggerFactory();

            // Register resource listing route
            var resourceListing = factory.CreateResourceListing(modules, this.ModulePath);
            Get["/"] = _ => CreateStreamedJsonResponse(resourceListing);

            // Register an api declaration route for each module
            var apiDelacations = modules.Select(module => factory.CreateApiDeclaration(module))
                                        .OrderBy(a => a.ResourcePath);

            foreach (var apiDeclaration in apiDelacations)
            {
                Get["/swagger" + apiDeclaration.BasePath] = _ => CreateStreamedJsonResponse(apiDeclaration);
            }
        }

        #endregion Constructors
    }
}