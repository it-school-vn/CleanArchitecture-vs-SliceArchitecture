#nullable disable
namespace VerticalSlice.Application.Entities
{
    public abstract class BaseTimestampEntity<T> : BaseEntity<T>, IHasTimestamp
    {
        public DateTimeOffset? Added { get; set; }
        public DateTimeOffset? Modified { get; set; }
    }
}