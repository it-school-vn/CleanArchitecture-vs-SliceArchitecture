using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace VerticalSlice.Application.Entities;

public sealed class TaskEntity : BaseTimestampEntity<Ulid>, IOwner, IDeletedEntity
{
    [Required]
    [MaxLength(250)]
    public required string Title { get; set; }

    [Required]
    public required string Description { get; set; }

    public Guid OwnerId { get; set; }

    public bool Deleted { get; set; }
}