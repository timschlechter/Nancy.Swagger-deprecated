namespace Nancy.Swagger.Providers.Attributes
{
    public class FromQueryAttribute : ParameterAttribute
    {
        public FromQueryAttribute(string name)
            : base(name, ParamType.query)
        {
        }
    }
}