using Nancy.ModelBinding;
using Nancy.Swagger.TestApp.Model;

namespace Nancy.Swagger.TestApp
{
    public class TestModule : NancyModule
    {
        public TestModule()
            : base("/Test")
        {
            Get["/Persons"] = parameters => GetPersons();
            Get["/Persons/{id}"] = parameters => GetPerson(Request.Query.id);
            Post["/Persons"] = parameters => PostPerson(this.Bind<Person>());

            Post["/NotDocumented"] = parameters => null;
        }

        [Get("/Persons/{id}",
            Summary = "Get all persons",
            Notes = "Lorem Ipsum is slechts een proeftekst uit het drukkerij- en zetterijwezen. Lorem Ipsum is de standaard proeftekst in deze bedrijfstak sinds de 16e eeuw, toen een onbekende drukker een zethaak met letters nam en ze door elkaar husselde om een font-catalogus te maken. Het heeft niet alleen vijf eeuwen overleefd maar is ook, vrijwel onveranderd, overgenomen in elektronische letterzetting. Het is in de jaren '60 populair geworden met de introductie van Letraset vellen met Lorem Ipsum passages en meer recentelijk door desktop publishing software zoals Aldus PageMaker die versies van Lorem Ipsum bevatten.",
            Type = typeof(Person))]
        private dynamic GetPerson([FromPath("id")] string id)
        {
            return new Person { Name = "John", Age = 30 };
        }

        [Get("/Persons",
            Summary = "Get all persons",
            Notes = "Lorem Ipsum is slechts een proeftekst uit het drukkerij- en zetterijwezen. Lorem Ipsum is de standaard proeftekst in deze bedrijfstak sinds de 16e eeuw, toen een onbekende drukker een zethaak met letters nam en ze door elkaar husselde om een font-catalogus te maken. Het heeft niet alleen vijf eeuwen overleefd maar is ook, vrijwel onveranderd, overgenomen in elektronische letterzetting. Het is in de jaren '60 populair geworden met de introductie van Letraset vellen met Lorem Ipsum passages en meer recentelijk door desktop publishing software zoals Aldus PageMaker die versies van Lorem Ipsum bevatten.",			
            Type = typeof(Person[]))]
        private dynamic GetPersons()
        {
            return new[] {
                new Person { Name = "John",  Age = 30 }
            };
        }

        [Post("/Persons",
            Summary = "Post a persons",
            Notes = "Lorem Ipsum is slechts een proeftekst uit het drukkerij- en zetterijwezen. Lorem Ipsum is de standaard proeftekst in deze bedrijfstak sinds de 16e eeuw, toen een onbekende drukker een zethaak met letters nam en ze door elkaar husselde om een font-catalogus te maken. Het heeft niet alleen vijf eeuwen overleefd maar is ook, vrijwel onveranderd, overgenomen in elektronische letterzetting. Het is in de jaren '60 populair geworden met de introductie van Letraset vellen met Lorem Ipsum passages en meer recentelijk door desktop publishing software zoals Aldus PageMaker die versies van Lorem Ipsum bevatten.",
            Type = typeof(Person)
        )]
        private dynamic PostPerson([FromBody] Person person)
        {
            return null;
        }
    }
}