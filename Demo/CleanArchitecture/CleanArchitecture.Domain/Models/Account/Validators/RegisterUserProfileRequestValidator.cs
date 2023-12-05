using CleanArchitecture.Domain.Enums;
using FluentValidation;
using CleanArchitecture.Domain.Models.Account.DTO;
using CleanArchitecture.Domain.Models.Account.Enum;
using CleanArchitecture.Domain.Models.Account.Validators;
using CleanArchitecture.Domain.Validators;

namespace CleanArchitecture.Domain.Models.Account.Validators;

public class RegisterUserProfileRequestValidator : AbstractValidator<RegisterUserProfileRequest>
{
        public RegisterUserProfileRequestValidator()
        {
                RuleFor(x => x.FirstName).NotEmpty()
                        .WithMessage("Your first name cannot be empty")
                        .MaximumLength(50)
                        .WithMessage("Your firt name length must not exceed 50");

                RuleFor(x => x.MiddleName).MaximumLength(50)
                        .When(x => !string.IsNullOrWhiteSpace(x.MiddleName))
                        .WithMessage("Your middle name length must not exceed 50");

                RuleFor(x => x.LastName).NotEmpty()
                        .WithMessage("Your last name cannot be empty")
                        .MaximumLength(50)
                        .WithMessage("Your last name length must not exceed 50");

                RuleFor(x => x.Title).NotEmpty().WithMessage("Your title cannot be empty")
                        .MaximumLength(100)
                        .WithMessage("Your title length must not exceed 100");

                RuleFor(x => x.AvartarUrl)
                        .SetInheritanceValidator(v => v.Add(new UrlValidator()));

                RuleFor(x => x.TimeZone).Must(x => x > -13 && x <= 15)
                .WithMessage("Your time zone (utc offset) must greater than -13 and not exceed 14");

                RuleFor(x => x.Skills).NotEmpty().WithMessage("Your skills cannot be empty")
                        .Must(x => x != null && x.Length < 25).WithMessage("Your number of skills must not exceed 25")
                        .Must(x => x != null && x.Any(y => y.Length <= 50)).WithMessage("Your skill must not execeed 50");

                RuleFor(x => x.JobStatus).IsInEnum().WithMessage("Your job status cannot be empty");


                RuleFor(x => x.InterviewDate)
                        .NotEmpty()
                        .When(x => x.JobStatus == JobStatus.Looking)
                        .WithMessage("Your interview date canot be empty")
                        .GreaterThan(DateOnly.FromDateTime(DateTime.Now)).WithMessage("Your interview date must greater than today");

                RuleFor(x => x.AccountType).IsInEnum()
                        .NotEmpty()
                        .WithMessage("Your account type cannot be empty")
                        .NotEqual(AccountType.None)
                        .WithMessage($"Your account type must be {nameof(AccountType.Student)}  or {nameof(AccountType.Profession)}");



                RuleFor(x => x.DesiredJob)
                        .NotEmpty().When(x => x.AccountType == AccountType.Student || x.AccountType == AccountType.Profession && x.JobStatus == JobStatus.Looking)
                        .WithMessage("Your desired job cannot be empty")
                        .MaximumLength(100).WithMessage("Your desired job length must not exceed 100");

                RuleFor(x => x.Student).Cascade(CascadeMode.Stop)
                        .NotEmpty().When(x => x.AccountType == AccountType.Student)
                        .WithMessage("Your student cannot be empty")
                        .SetInheritanceValidator(v =>
                        {
                                v.Add(new StudentInfoValidator());
                        });


                RuleFor(x => x.Profession).Cascade(CascadeMode.Stop)
                .NotEmpty().When(x => x.AccountType == AccountType.Profession)
                .WithMessage("Your professional cannot be empty")
                .SetInheritanceValidator(v =>
                {
                        v.Add(new ProfessionInfoValidator());
                });
        }

}