namespace EventSourcingTutorial.Events;

public class StudentUnEnrolled : Event
{
    public Guid StudentId { get; set; }
    public required string CourseName { get; set; }
    public override Guid StreamId => StudentId;
}