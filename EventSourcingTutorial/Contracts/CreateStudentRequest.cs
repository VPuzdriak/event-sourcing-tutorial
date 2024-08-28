namespace EventSourcingTutorial.Contracts;

public record CreateStudentRequest(string FullName, string Email, DateTime DateOfBirth);