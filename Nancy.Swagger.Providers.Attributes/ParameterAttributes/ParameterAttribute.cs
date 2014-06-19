using System;

namespace Nancy.Swagger.Providers.Attributes
{
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