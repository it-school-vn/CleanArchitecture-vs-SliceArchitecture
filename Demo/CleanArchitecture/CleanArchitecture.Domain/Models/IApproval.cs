using CleanArchitecture.Domain.ValueObjects.ComplexType;

namespace CleanArchitecture.Domain.Entities;

public interface IApproval
{
    Approval Approval { get; set; }
}