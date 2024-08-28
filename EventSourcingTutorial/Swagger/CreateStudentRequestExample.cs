using EventSourcingTutorial.Contracts;
using Swashbuckle.AspNetCore.Filters;

namespace EventSourcingTutorial.Swagger;

public class CreateStudentRequestExample : IExamplesProvider<CreateStudentRequest>
{
    public CreateStudentRequest GetExamples() =>
        new("Volodymyr Puzdriak", "puzdriak@mail.com", new DateTime(1996, 1, 22));
}