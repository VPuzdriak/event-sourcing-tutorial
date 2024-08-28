using MongoDB.Bson.Serialization.Attributes;

namespace EventSourcingTutorial.Events;

public abstract class Event
{
    [BsonId] public Guid Id { get; set; }
    [BsonElement] public abstract Guid StreamId { get; }
    public DateTime CreatedAtUtc { get; set; }
}