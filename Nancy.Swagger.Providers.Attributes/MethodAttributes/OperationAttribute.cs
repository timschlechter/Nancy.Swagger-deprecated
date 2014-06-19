using System;

namespace Nancy.Swagger.Providers.Attributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class OperationAttribute : Attribute
    {
        #region Constructors

        public OperationAttribute(string method, string path)
        {
            Method = method;
            Path = path;
        }

        #endregion Constructors

        #region Properties

        public string Method { get; set; }

        public string Notes { get; set; }

        public string Path { get; set; }

        public string Summary { get; set; }

        public Type Type { get; set; }

        #endregion Properties
    }
}