using System.ComponentModel.DataAnnotations;


namespace CleanArchitecture.Domain.Models.Account.ValueObjects;

public record EducationInfo([MaxLength(250)] string SchoolName, [MaxLength(250)] string Majors, bool IsCurrent, DateOnly From, DateOnly? To, [MaxLength(450)] string Description);