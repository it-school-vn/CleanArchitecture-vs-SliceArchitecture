using CleanArchitecture.Domain.Enums;
using CleanArchitecture.Domain.Models.Account.Enum;
using CleanArchitecture.Domain.Models.Account.ValueObjects;

namespace CleanArchitecture.Domain.Models.Account.DTO;

public class RegisterUserProfileRequest
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? MiddleName { get; set; }
    public string? Title { get; set; }
    public string? AvartarUrl { get; set; }
    public int TimeZone { get; set; } = 7;
    public string[]? Skills { get; set; }
    public JobStatus? JobStatus { get; set; }
    public string? DesiredJob { get; set; }
    public DateOnly? InterviewDate { get; set; }
    public AccountType? AccountType { get; set; }

    public StudentInfo? Student { get; set; }
    public ProfessionInfo? Profession { get; set; }
}