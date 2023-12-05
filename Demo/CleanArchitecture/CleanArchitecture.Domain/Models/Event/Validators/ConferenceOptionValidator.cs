using FluentValidation;
using CleanArchitecture.Domain.Enums;
using CleanArchitecture.Domain.Models.Event.ValueObjects;

namespace CleanArchitecture.Domain.Validators;

public class ConferenceOptionValidator : AbstractValidator<ConferenceOption>
{
    public ConferenceOptionValidator()
    {
        RuleFor(x => x.Type)
           .IsInEnum()
           .WithMessage("Your conference type is invalid");

        RuleFor(x => x.Tool)
       .IsInEnum()
       .WithMessage("Your conference tool is invalid");

        RuleFor(x => x.Url).NotEmpty()
       .When(x => x.Type == MeetingType.Online)
       .WithMessage("Your conference url cannot be empty");

        RuleFor(x => x.PassCode)
        .MaximumLength(25).WithMessage("Your conference passcode length cannot exceed 25");


        RuleFor(x => x.MeetingId)
        .MaximumLength(25).WithMessage("Your conference meeting Id length cannot exceed 50");
    }
}