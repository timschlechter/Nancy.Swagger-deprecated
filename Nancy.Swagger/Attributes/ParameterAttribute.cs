using Nancy.Swagger.Model;
using System;
using System.Collections.Generic;

namespace Nancy.Swagger
{
    [AttributeUsage(AttributeTargets.Parameter, Inherited=true)]
    public class ParameterAttribute : Attribute
    {
        #region Constructors

        public ParameterAttribute(string name, ParamType paramType)
        {
            Name = name;
            ParamType = paramType;
        }

        #endregion

        #region Properties

        public string Name { get; set; }

        public ParamType ParamType { get; set; }

        public bool Required { get; set; }

        #endregion
    }

    public class FromQueryAttribute : ParameterAttribute
    {
        public FromQueryAttribute(string name) : base(name, ParamType.query) { }
    }

    public class FromBodyAttribute : ParameterAttribute
    {
        public FromBodyAttribute() : base("body", ParamType.body) { }
    }

    public class FromPathAttribute : ParameterAttribute
    {
        public FromPathAttribute(string name) : base(name, ParamType.path) {
            Required = true;
        }
    }
}