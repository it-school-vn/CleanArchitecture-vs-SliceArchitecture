using FluentValidation;
using CleanArchitecture.Domain.Enums;
using CleanArchitecture.Domain.Models.Event.DTO;
using CleanArchitecture.Domain.Validators;
using CleanArchitecture.Domain.ValueObjects.ComplexType;

namespace CleanArchitecture.Application.Features.Event.Validators;

public class CreateOrUpdateEventRequestValidator : AbstractValidator<CreateOrUpdateEventRequest>
{
    public CreateOrUpdateEventRequestValidator()
    {
        RuleFor(x => x.Title).NotEmpty()
                                  .WithMessage("Your title cannot be empty")
                                  .MaximumLength(250)
                                  .WithMessage("Your title length must not exceed 250");

        RuleFor(x => x.Description).NotEmpty()
            .WithMessage("Your description cannot be empty");

        RuleFor(x => x.BannerUrl)
        .SetInheritanceValidator(x => x.Add(new UrlValidator()))
        .When(x => !string.IsNullOrWhiteSpace(x.BannerUrl));

        RuleFor(x => x.Fee).Must(fee => fee >= 0).WithMessage("Your event fee can not be negative number");

        RuleFor(x => x.EventType).IsInEnum().WithMessage("Your event type is invalid");

        RuleFor(x => x.Conference).Cascade(CascadeMode.Stop)
        .NotNull().WithMessage("Your conference option cannot be empty")
        .SetInheritanceValidator(v =>
        {
            v.Add(new ConferenceOptionValidator());
        });

        RuleFor(x => x.Location).Cascade(CascadeMode.Stop)
        .NotNull().WithMessage("Your localtion cannot be empty")
        .SetInheritanceValidator(v =>
        {
            v.Add<Address>(new AddressValidator());
        })
        .When(x => x.Conference is not null && x.Conference!.Type == MeetingType.Offline);

        RuleFor(x => x.ClosedDate).NotEmpty()
        .WithMessage("Your closed date cannot be empty");

        RuleFor(x => x.ClosedTime).NotEmpty()
               .WithMessage("Your closed tome cannot be empty");


        RuleFor(x => x.OpenDate).NotEmpty()
        .WithMessage("Your open date cannot be empty");

        RuleFor(x => x.OpenTime).NotEmpty()
        .WithMessage("Your open time cannot be empty");

        RuleFor(x => x.DateAt).NotEmpty()
        .WithMessage("Your date at cannot be empty");


        RuleFor(x => x.TimeAt).NotEmpty()
        .WithMessage("Your time at cannot be empty");

        RuleFor(x => x.Duration).Must(x => x >= 15)
        .WithMessage("Your duration must be greater or equal to 15");
    }
}