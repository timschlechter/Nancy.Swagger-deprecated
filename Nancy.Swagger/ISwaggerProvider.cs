using Nancy.Swagger.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nancy.Swagger
{
    public interface ISwaggerProvider
    {
        ResourceListing GetResourceListing(string basePath);

        IEnumerable<ApiDeclaration> GetApiDeclarations();
    }
}
