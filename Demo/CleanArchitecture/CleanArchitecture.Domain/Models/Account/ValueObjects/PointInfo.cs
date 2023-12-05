using System.ComponentModel.DataAnnotations.Schema;
using CleanArchitecture.Domain.Models.Account.Enum;

namespace CleanArchitecture.Domain.Models.Account.ValueObjects
{
    [ComplexType]
    public record PointInfo(int Total)
    {
        public AccountLevel AccountLevel => Total switch
        {
            _ when Total >= 1000 => AccountLevel.Silver,
            _ when Total >= 3000 => AccountLevel.Bronze,
            _ when Total >= 10000 => AccountLevel.Gold,
            _ => AccountLevel.None
        };
    }
}