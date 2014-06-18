using System;

namespace Nancy.Swagger
{
    public class FromBodyAttribute : ParameterAttribute
    {
        public FromBodyAttribute()
            : base("body", ParamType.body)
        {
        }
    }

    public class FromHeaderAttribute : ParameterAttribute
    {
        public FromHeaderAttribute(string name)
            : base(name, ParamType.header)
        {
        }
    }

    public class FromPathAttribute : ParameterAttribute
    {
        public FromPathAttribute(string name)
            : base(name, ParamType.path)
        {
            Required = true;
        }
    }

    public class FromQueryAttribute : ParameterAttribute
    {
        public FromQueryAttribute(string name)
            : base(name, ParamType.query)
        {
        }
    }

    [AttributeUsage(AttributeTargets.Parameter, Inherited = true)]
    public class ParameterAttribute : Attribute
    {
        #region Constructors

        public ParameterAttribute(string name, ParamType paramType)
        {
            Name = name;
            ParamType = paramType;
        }

        #endregion Constructors

        #region Properties

        public string Name { get; set; }

        public ParamType ParamType { get; set; }

        public bool Required { get; set; }

        #endregion Properties
    }
}