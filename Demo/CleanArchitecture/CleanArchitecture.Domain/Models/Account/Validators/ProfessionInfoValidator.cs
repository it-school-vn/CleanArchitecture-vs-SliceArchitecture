using FluentValidation;
using CleanArchitecture.Domain.Models.Account.ValueObjects;

namespace CleanArchitecture.Domain.Models.Account.Validators;

public class ProfessionInfoValidator : AbstractValidator<ProfessionInfo>
{
    public ProfessionInfoValidator()
    {

        RuleFor(x => x.Company).Cascade(CascadeMode.Stop)
             .NotEmpty()
             .WithMessage("Your company cannot be empty")
             .MaximumLength(200)
             .WithMessage("Your company lenght must not exceed 200");

        RuleFor(x => x.YearOfExperience).Cascade(CascadeMode.Stop)
        .NotEmpty().WithMessage("Your year of experience cannot be empty")
        .GreaterThanOrEqualTo(0)
        .WithMessage("Your year of experience must >= 0")
        .LessThanOrEqualTo(50)
        .WithMessage("Your year of experience must <= 50");
    }
}