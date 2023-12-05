using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CleanArchitecture.Domain.Models.Account.ValueObjects;

[ComplexType]
public record StudentInfo([MaxLength(200)] string? School, int? YearOfGraduation)
{ }
