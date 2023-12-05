namespace VerticalSlice.Application.Entities;

public interface IHasTimestamp
{
    DateTimeOffset? Added { get; set; }
    DateTimeOffset? Modified { get; set; }
}