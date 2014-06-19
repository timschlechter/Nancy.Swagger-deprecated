namespace Nancy.Swagger.Providers.Attributes
{
    public class FromBodyAttribute : ParameterAttribute
    {
        public FromBodyAttribute()
            : base("body", ParamType.body)
        {
        }
    }
}