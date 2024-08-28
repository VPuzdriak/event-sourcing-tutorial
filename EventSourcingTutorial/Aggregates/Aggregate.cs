using EventSourcingTutorial.Events;

namespace EventSourcingTutorial.Aggregates;

public abstract class Aggregate
{
    public abstract void Apply(Event @event);
}