using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Domain.Models.FeatureFlag;

public sealed class FeatureFlagEntity : BaseTimestampEntity<Guid>
{
    [Required]
    [MaxLength(25), Unicode(false)]
    public required string Name { get; set; }

    [MaxLength(200)]
    public string? Description { get; set; }

    [Required]
    public required bool Enable { get; set; }
}