using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CleanArchitecture.Domain.ValueObjects.ComplexType;

[ComplexType]
public record Address([MaxLength(450)] string Line,
[MaxLength(25)] string Ward,
[MaxLength(25)] string District,
[MaxLength(25)] string City,
[MaxLength(25)] string Country,
[MaxLength(450)] string? Line2,
[MaxLength(25)] string? State,
[MaxLength(15)] string? ZipCode
)
{ }