using System.Reflection;

namespace Nancy.Swagger.Providers.Attributes
{
    internal class DocumentedMethod
    {
        public MethodBase Method { get; set; }

        public OperationAttribute OperationAttribute { get; set; }
    }
}