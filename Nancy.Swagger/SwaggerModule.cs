using Nancy.IO;
using Newtonsoft.Json;
using System.IO;
using System.Linq;

namespace Nancy.Swagger
{
    public class SwaggerModule : NancyModule
    {
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

        public SwaggerModule(ISwaggerProvider provider)
            : base(StaticConfiguration.ModulePath)
        {
            // Register resource listing route
            Get["/"] = _ => CreateStreamedJsonResponse(provider.GetResourceListing(this.ModulePath));

            foreach (var apiDeclaration in provider.GetApiDeclarations())
            {
                Get["/swagger" + apiDeclaration.BasePath] = _ => CreateStreamedJsonResponse(apiDeclaration);
            }
        }

        #endregion Constructors
    }
}