using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CleanArchitecture.Domain.ValueObjects.ComplexType;

[ComplexType]
public record WithIdName<T>(T Id, [MaxLength(250)] string Name);
