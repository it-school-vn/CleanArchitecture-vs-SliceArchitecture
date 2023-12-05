namespace CleanArchitecture.Domain.Entities;

public interface IDeletedEntity
{
    bool Deleted { get; set; }
}