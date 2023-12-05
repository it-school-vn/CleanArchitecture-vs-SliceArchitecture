using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CleanArchitecture.Domain.Enums;

namespace CleanArchitecture.Domain.Models.Event.ValueObjects;

[ComplexType]
public record ConferenceOption(MeetingType Type,
 ConferenceTool Tool,
[MaxLength(2000)] string Url, [MaxLength(25)] string? PassCode, [MaxLength(50)] string? MeetingId)
{
}