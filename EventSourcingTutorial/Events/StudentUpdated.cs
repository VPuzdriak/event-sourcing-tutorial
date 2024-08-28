namespace EventSourcingTutorial.Events;

public class StudentUpdated : Event
{
    public Guid StudentId { get; set; }
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public override Guid StreamId => StudentId;
}