using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Domain.ValueObjects;

public record WithNameValue([MaxLength(250)] string Name, [MaxLength(2048)] string Value);
