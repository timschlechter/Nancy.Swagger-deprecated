namespace Nancy.Swagger.Providers.Attributes
{
    public class FromHeaderAttribute : ParameterAttribute
    {
        public FromHeaderAttribute(string name)
            : base(name, ParamType.header)
        {
        }
    }
}