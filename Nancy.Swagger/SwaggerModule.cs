using Nancy.Json;
using Nancy.Serialization.JsonNet;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;

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
            new JsonNetSerializer().Serialize(contentType, deserialized, stream);
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

        public SwaggerModule()
            : base(StaticConfiguration.ModulePath)
        {
            var modules = _discoverer.GetModulesToDocument();

            var factory = new SwaggerFactory();

            // Register resource listing route
            var resourceListing = factory.CreateResourceListing(modules, this.ModulePath);
            Get["/"] = _ => CreateStreamedJsonResponse(resourceListing);

            // Register an api declaration route for each module
            var apiDelacations = modules.Select(module => factory.CreateApiDeclaration(module));
            foreach (var apiDeclaration in apiDelacations)
            {
                Get[apiDeclaration.BasePath] = _ => CreateStreamedJsonResponse(apiDeclaration);
            }
        }                   

        #endregion Constructors
    }
}