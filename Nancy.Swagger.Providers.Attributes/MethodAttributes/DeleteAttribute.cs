namespace Nancy.Swagger.Providers.Attributes
{
    public class Delete : OperationAttribute
    {
        public Delete(string path)
            : base("DELETE", path)
        {
        }
    }
}