using Swashbuckle.AspNetCore.Filters;

namespace EventSourcingTutorial.Swagger;

public class EnrollRequestExample : IExamplesProvider<EnrollRequest>
{
    public EnrollRequest GetExamples() =>
        new(Guid.NewGuid(), "Event Sourcing from zero to hero");
}