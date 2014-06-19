namespace Nancy.Swagger.Providers.Attributes
{
    public class Get : OperationAttribute
    {
        public Get(string path)
            : base("GET", path)
        {
        }
    }
}