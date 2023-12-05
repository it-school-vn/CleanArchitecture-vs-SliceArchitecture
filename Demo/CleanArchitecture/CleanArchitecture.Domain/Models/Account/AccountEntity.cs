using System.ComponentModel.DataAnnotations;
using CleanArchitecture.Domain.Constants;
using CleanArchitecture.Domain.Enums;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Models.Account.Enum;
using CleanArchitecture.Domain.Models.Account.ValueObjects;
using CleanArchitecture.Domain.ValueObjects.ComplexType;

namespace CleanArchitecture.Domain.Models.Account
{
    public sealed class AccountEntity : BaseTimestampEntity<Guid>, IHasTimestamp, IApproval
    {

        [Required]
        [MaxLength(50)]
        public required string Email { get; set; }

        [Required]
        public AccountStatus Status { get; set; }

        [Required]
        public AccountType AccountType { get; set; } = AccountType.None;

        [Required]
        [MaxLength(25)]
        public string Role { get; set; } = Global.AccountRole.User;

        [MaxLength(50)]
        [Required]
        public required string FirstName { get; set; }

        [MaxLength(50)]
        public string? MiddleName { get; set; }

        [MaxLength(50)]
        [Required]
        public required string LastName { get; set; }

        [Required]
        public byte TimeZone { get; set; } = 7;


        [MaxLength(450)]
        public string[]? Skills { get; set; }

        [MaxLength(100)]
        public string? DesiredJob { get; set; }
        public DateOnly? InterviewDate { get; set; }

        [MaxLength(100)]
        public string? Title { get; set; }

        [MaxLength(2000)]
        public string? AvatarUrl { get; set; }

        public JobStatus JobStatus { get; set; } = JobStatus.None;

        [Required]
        public required StudentInfo Student { get; set; }

        [Required]
        public required ProfessionInfo Profession { get; set; }

        [Required]
        public required PointInfo Point { get; set; } = new PointInfo(0);

        [Required]
        public Approval Approval { get; set; } = new Approval(true, string.Empty);

        [MaxLength(2500)]
        public string? About { get; set; }

        public ICollection<EducationInfo>? EducationInfos { get; set; }

    }
}