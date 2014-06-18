using Nancy.Json;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using Nancy.IO;

namespace Nancy.Swagger
{
    public class SwaggerModule : NancyModule
    {
        private NancyApiDiscoverer _discoverer = new NancyApiDiscoverer();

        #region Static Helpers

        private static string ContentType
        {
            get
            {
                return "application/json" + (String.IsNullOrWhiteSpace(JsonSettings.DefaultCharset) ? "" : "; charset=" + JsonSettings.DefaultCharset);
            }
        }

        private static void SerializeJson<TModel>(string contentType, TModel model, Stream stream)
        {
            // HACK: serialize and deserialize the object to make sure JsonProperty attributes are processed
            var serialized = JsonConvert.SerializeObject(
                value: model,
                settings: new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                }
            );
            var deserialized = JsonConvert.DeserializeObject(serialized);

            // Serialize the model to the stream
            var serializer = new JsonSerializer();
			
			using (var writer = new JsonTextWriter(new StreamWriter(new UnclosableStreamWrapper(stream))))
			{
				serializer.Serialize(writer, deserialized);
			}
        }

        private Response CreateStreamedJsonResponse(dynamic model)
        {
            return new Response
            {
                ContentType = "application/json",
                StatusCode = HttpStatusCode.OK,
                Contents = stream => SerializeJson(
                    ContentType,
                    model,
                    stream
                )
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