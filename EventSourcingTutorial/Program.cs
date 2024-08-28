using EventSourcingTutorial;
using EventSourcingTutorial.Contracts;
using EventSourcingTutorial.Events;
using EventSourcingTutorial.Mongo;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMongoDb(builder.Configuration);
builder.Services.AddScoped<StudentDatabase>();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c => { c.ExampleFilters(); });

builder.Services.AddSwaggerExamplesFromAssemblyOf<Program>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/events", (StudentDatabase studentDatabase) => studentDatabase.GetEvents());

app.MapGet("/student/{id:guid}", async (Guid id, StudentDatabase studentDatabase) =>
{
    var student = await studentDatabase.GetStudentAsync(id);
    return student is null ? Results.NotFound() : Results.Ok(student);
});

app.MapPost("/students", async (CreateStudentRequest request, StudentDatabase studentDatabase) =>
{
    var studentCreated = new StudentCreated
    {
        Id = Guid.NewGuid(),
        StudentId = Guid.NewGuid(),
        FullName = request.FullName,
        Email = request.Email,
        DateOfBirth = request.DateOfBirth
    };

    await studentDatabase.AppendAsync(studentCreated);

    return Results.Ok(studentCreated.StudentId);
});

app.MapPut("/student", async (UpdateStudentRequest request, StudentDatabase studentDatabase) =>
{
    var studentUpdated = new StudentUpdated
    {
        Id = Guid.NewGuid(),
        StudentId = request.StudentId,
        FullName = request.FullName,
        Email = request.Email,
    };

    await studentDatabase.AppendAsync(studentUpdated);
});

app.MapPost("/students/enroll", async (EnrollRequest request, StudentDatabase studentDatabase) =>
{
    var studentEnrolled = new StudentEnrolled
    {
        Id = Guid.NewGuid(),
        StudentId = request.StudentId,
        CourseName = request.CourseName
    };

    await studentDatabase.AppendAsync(studentEnrolled);
});

app.Run();