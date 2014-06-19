namespace Nancy.Swagger.Providers.Attributes
{
    public class Post : OperationAttribute
    {
        public Post(string path)
            : base("POST", path)
        {
        }
    }
}