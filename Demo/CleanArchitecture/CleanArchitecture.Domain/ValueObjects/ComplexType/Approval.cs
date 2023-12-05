using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CleanArchitecture.Domain.ValueObjects.ComplexType;

[ComplexType]
public record Approval(bool Yes = true, [MaxLength(450)] string Reason = "");
