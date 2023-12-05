using CleanArchitecture.Domain.Models.Account.Enum;
using CleanArchitecture.Domain.Models.Account.ValueObjects;

namespace CleanArchitecture.Domain.Models.Account.DTO
{
    public sealed record UserResponse(
           string Email,
           string Title,
           string FirstName,
           string? MiddleName,
           string LastName,
           string Role,
           int TimeZone,
           AccountStatus Status,
           AccountType AccountType,
           StudentInfo? Student,
           ProfessionInfo? Profession,
           string[] Skills
       );
}