using EventSourcingTutorial.Aggregates;
using EventSourcingTutorial.Events;
using MongoDB.Driver;

namespace EventSourcingTutorial;

public class StudentDatabase
{
    private readonly IMongoCollection<Event> _eventsCollection;
    private readonly IMongoCollection<Student> _studentsCollection;

    public StudentDatabase(IMongoDatabase mongoDatabase)
    {
        _eventsCollection = mongoDatabase.GetCollection<Event>("events");
        _studentsCollection = mongoDatabase.GetCollection<Student>("students");
    }

    public async Task AppendAsync<T>(T @event) where T : Event
    {
        @event.CreatedAtUtc = DateTime.UtcNow;
        await _eventsCollection.InsertOneAsync(@event);

        var student = await AggregateEvents<Student>(@event.StreamId);

        var filter = Builders<Student>.Filter.Eq(x => x.Id, @event.StreamId);
        await _studentsCollection.ReplaceOneAsync(filter, student!, new ReplaceOptions { IsUpsert = true });
    }

    public async Task<List<Event>> GetEvents()
    {
        var filter = Builders<Event>.Filter.Empty;
        var sort = Builders<Event>.Sort.Ascending(x => x.StreamId).Ascending(x => x.CreatedAtUtc);
        return await _eventsCollection.Find(filter).Sort(sort).ToListAsync();
    }

    public async Task<Student?> GetStudentAsync(Guid id)
    {
        var filter = Builders<Student>.Filter.Eq(x => x.Id, id);
        return await _studentsCollection.Find(filter).FirstOrDefaultAsync();
    }

    private async Task<T?> AggregateEvents<T>(Guid id) where T : Aggregate, new()
    {
        var filter = Builders<Event>.Filter.Eq(x => x.StreamId, id);
        var sort = Builders<Event>.Sort.Ascending(x => x.StreamId).Ascending(x => x.CreatedAtUtc);
        var events = await _eventsCollection.Find(filter).Sort(sort).ToListAsync();

        var aggregate = new T();

        foreach (var @event in events)
        {
            aggregate.Apply(@event);
        }

        return aggregate;
    }
}