[![Build status](https://ci.appveyor.com/api/projects/status/9lg2fvqccaxa2ssg/branch/master)](https://ci.appveyor.com/project/TimSchlechter/nancy-swagger)

Add the [Nancy.Swagger package](https://www.nuget.org/packages/Nancy.Swagger/) to your API application to add an endpoint which provides a [Swagger](https://helloreverb.com/developers/swagger) [1.2 description](https://github.com/wordnik/swagger-spec/blob/master/versions/1.2.md) of your API.

## Usage
```
public class Bootstrapper : DefaultNancyBootstrapper
{
    protected override void ApplicationStartup(TinyIoc.TinyIoCContainer container, IPipelines pipelines)
    {
        base.ApplicationStartup(container, pipelines);
        
        // Configure the Swagger endpoint's path
        Nancy.Swagger.StaticConfiguration.ModulePath = "/api";
    }
}
```

The module will automatically discover all relevant Nancy modules in your application and return a [Resource Listing](https://github.com/wordnik/swagger-spec/blob/master/versions/1.2.md#51-resource-listing) at the given configured ModulePath. 

It also adds an endpoint for each Nancy module, at which an [API Declaration](https://github.com/wordnik/swagger-spec/blob/master/versions/1.2.md#52-api-declaration) will be returned.

The Resource Listing endpoint can be used in [Swagger UI](https://github.com/wordnik/swagger-ui).

## Make your documentation complete
Only the basic information about your API can be discovered automatically. Annotate your API handlers with attributes to add extra information:
```
public class PersonModule : NancyModule
{
    public PersonModule() : base("/Persons")
    {
        Get["/"] = parameters => GetPersons();
        Post["/"] = parameters => PostPerson(this.Bind<Person>());
    }

    [Get("/Persons/{id}",
        Summary = "Returns the person with the given {id}",
        Notes = "Some notes",
        Type = typeof(Person))]
    private dynamic GetPerson([FromPath("id")] string id) { }

    [Post("/Persons",
        Summary = "Create a new person"
        Type = typeof(Person)
    )]
    private dynamic PostPerson([FromBody] Person person) { }
}
```

Currently supported attributes:
  - Methods attributes
      - [Get]
      - [Post]
      - [Put]
      - [Delete]
  - Parameter attributes:
      - [FromBody]
      - [FromQuery]
      - [FromPath]
      - [FromHeader]

## Known issues
* Only works correct when your application uses the [Json.Net serializer](http://james.newtonking.com/json) at the moment