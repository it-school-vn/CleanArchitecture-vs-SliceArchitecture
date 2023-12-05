using FluentValidation;
using CleanArchitecture.Domain.ValueObjects.ComplexType;

namespace CleanArchitecture.Domain.Validators;

public class AddressValidator : AbstractValidator<Address>
{
    public AddressValidator()
    {
        RuleFor(x => x.Line)
        .NotEmpty()
        .WithMessage("Your address line cannot be empty")
        .MaximumLength(450).WithMessage("Your address line length must not exceed 450");

        RuleFor(x => x.Ward)
              .NotEmpty()
              .WithMessage("Your ward cannot be empty")
              .MaximumLength(25).WithMessage("Your ward length must not exceed 25");

        RuleFor(x => x.District)
                   .NotEmpty()
                   .WithMessage("Your district cannot be empty")
                   .MaximumLength(25).WithMessage("Your district length must not exceed 25");

        RuleFor(x => x.City)
        .NotEmpty()
        .WithMessage("Your city cannot be empty")
        .MaximumLength(25).WithMessage("Your city length must not exceed 25");


        RuleFor(x => x.Country)
        .NotEmpty()
        .WithMessage("Your country cannot be empty")
        .MaximumLength(25).WithMessage("Your country length must not exceed 25");

    }
}