namespace Nancy.Swagger.Providers.Attributes
{
    public class FromPathAttribute : ParameterAttribute
    {
        public FromPathAttribute(string name)
            : base(name, ParamType.path)
        {
            Required = true;
        }
    }
}