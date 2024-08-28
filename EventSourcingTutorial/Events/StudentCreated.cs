namespace EventSourcingTutorial.Events;

public class StudentCreated : Event
{
    public required Guid StudentId { get; set; }
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public required DateTime DateOfBirth { get; set; }
    public override Guid StreamId => StudentId;
}