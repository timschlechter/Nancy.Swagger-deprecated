namespace Nancy.Swagger.Providers.Attributes
{
    public class Put : OperationAttribute
    {
        public Put(string path)
            : base("PUT", path)
        {
        }
    }
}