using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CleanArchitecture.Domain.Models.Account.ValueObjects;

[ComplexType]
public record ProfessionInfo([MaxLength(200)] string? Company, int? YearOfExperience)
{
}
