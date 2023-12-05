using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Domain.ValueObjects;

public record WithName([MaxLength(250)] string Name);
