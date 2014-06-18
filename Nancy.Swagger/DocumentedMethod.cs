using System.Reflection;

namespace Nancy.Swagger
{
    public class DocumentedMethod
    {
        public MethodBase Method { get; set; }

        public OperationAttribute OperationAttribute { get; set; }
    }
}