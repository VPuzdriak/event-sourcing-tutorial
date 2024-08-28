using EventSourcingTutorial.Events;

namespace EventSourcingTutorial.Aggregates;

public class Student : Aggregate
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public List<string> EnrolledCourses { get; set; } = [];
    public DateTime DateOfBirth { get; set; }

    public void Apply(StudentCreated @event)
    {
        Id = @event.StudentId;
        FullName = @event.FullName;
        Email = @event.Email;
        DateOfBirth = @event.DateOfBirth;
    }

    public override void Apply(Event @event)
    {
        switch (@event)
        {
            case StudentCreated studentCreated:
                Apply(studentCreated);
                break;
            case StudentUpdated studentUpdated:
                Apply(studentUpdated);
                break;
            case StudentEnrolled studentEnrolled:
                Apply(studentEnrolled);
                break;
            case StudentUnEnrolled studentUnEnrolled:
                Apply(studentUnEnrolled);
                break;
        }
    }

    private void Apply(StudentUpdated @event)
    {
        FullName = @event.FullName;
        Email = @event.Email;
    }

    private void Apply(StudentEnrolled @event)
    {
        if (!EnrolledCourses.Contains(@event.CourseName))
        {
            EnrolledCourses.Add(@event.CourseName);
        }
    }

    private void Apply(StudentUnEnrolled @event)
    {
        if (EnrolledCourses.Contains(@event.CourseName))
        {
            EnrolledCourses.Remove(@event.CourseName);
        }
    }
}