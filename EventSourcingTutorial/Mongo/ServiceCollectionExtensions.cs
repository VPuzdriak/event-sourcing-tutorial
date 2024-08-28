using EventSourcingTutorial.Events;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace EventSourcingTutorial.Mongo;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("MongoDb");
        var client = new MongoClient(connectionString);

        services.AddSingleton<IMongoClient>(client);
        services.AddScoped<IMongoDatabase>(sp => sp.GetRequiredService<IMongoClient>().GetDatabase("StudentsDb"));

        // Register class maps for derived event types
        BsonClassMap.RegisterClassMap<StudentCreated>(cm =>
        {
            cm.AutoMap();
            cm.SetDiscriminator(nameof(StudentCreated));
        });
        
        BsonClassMap.RegisterClassMap<StudentUpdated>(cm =>
        {
            cm.AutoMap();
            cm.SetDiscriminator(nameof(StudentUpdated));
        });
        
        BsonClassMap.RegisterClassMap<StudentEnrolled>(cm =>
        {
            cm.AutoMap();
            cm.SetDiscriminator(nameof(StudentEnrolled));
        });
        
        BsonClassMap.RegisterClassMap<StudentUnEnrolled>(cm =>
        {
            cm.AutoMap();
            cm.SetDiscriminator(nameof(StudentUnEnrolled));
        });

        return services;
    }
}