using Nancy.Swagger.Model;
using System;
using System.Collections.Generic;

namespace Nancy.Swagger
{
    [AttributeUsage(AttributeTargets.Method, Inherited=true)]    
    public class OperationAttribute : Attribute
    {
        #region Constructors
        
        public OperationAttribute(string method, string path)
        {
            Method = method;
            Path = path;
        }

        #endregion

        #region Properties
        
        public string Method { get; set; }

        public string Notes { get; set; }

        public string Path { get; set; }

        public string Summary { get; set; }

        public Type Type { get; set; }

        #endregion
    }

    public class Get : OperationAttribute
    {
        public Get(string path) : base("GET", path) { }
    }

    public class Post : OperationAttribute
    {
        public Post(string path) : base("POST", path) { }
    }

    public class Put : OperationAttribute
    {
        public Put(string path) : base("PUT", path) { }
    }

    public class Delete : OperationAttribute
    {
        public Delete(string path) : base("DELETE", path) { }
    }
}