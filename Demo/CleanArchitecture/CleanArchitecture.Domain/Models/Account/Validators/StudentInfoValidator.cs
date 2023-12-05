using FluentValidation;
using CleanArchitecture.Domain.Models.Account.ValueObjects;

namespace CleanArchitecture.Domain.Models.Account.Validators;

public class StudentInfoValidator : AbstractValidator<StudentInfo>
{
    public StudentInfoValidator()
    {

        RuleFor(x => x.School).Cascade(CascadeMode.Stop)
        .NotEmpty()
        .WithMessage("Your school cannot be empty")
        .MaximumLength(200)
        .WithMessage("Your school lenght must not exceed 200");

        RuleFor(x => x.YearOfGraduation).Cascade(CascadeMode.Stop)
        .NotEmpty().WithMessage("Your year of graduation cannot be empty")
        .GreaterThan(1950)
        .WithMessage("Your year of graduation must greater 1950");
    }
}