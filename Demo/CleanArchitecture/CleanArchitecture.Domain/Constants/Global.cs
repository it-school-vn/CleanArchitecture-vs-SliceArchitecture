using System.Security.Claims;
namespace CleanArchitecture.Domain.Constants
{
    public static class Global
    {
        public const int PageSize = 20;

        public const int MaxPageSize = 100;
        public const int UrlLength = 2000;

        public static class Claims
        {

            public const string UserId = "uid";
            public const string AccountType = "acc_type";
            public const string AccountStatus = "acc_status";
            public const string NameIdentifier = ClaimTypes.NameIdentifier;
            public const string FullName = Name;
            public const string AvatarUrl = "avatar_url";
            public const string IpAddress = "ipAddress";
            public const string Scope = "scope";
            public const string Email = ClaimTypes.Email;
            public const string Role = ClaimTypes.Role;
            public const string Sub = "sub";

            public const string Iss = "iss";

            public const string Iat = "iat";

            public const string ExpiredAt = ClaimTypes.Expired;

            public const string FamilyName = ClaimTypes.Surname;
            public const string GivenName = ClaimTypes.GivenName;

            public const string MiddleName = "middle_name";

            public const string Name = ClaimTypes.Name;

            public const string HostedDomain = "hd";

            public const string MobilePhone = ClaimTypes.MobilePhone;

            public const string EmailVerified = "email_verified";


        }

        public static class ApiScope
        {
            public const string Admin = "admin_api";
            public const string User = "user_api";

            public const string None = "none";
        }

        public static class AccountRole
        {

            public const string User = nameof(User);
            public const string Admin = nameof(Admin);
        }

        public static class Policy
        {
            public const string AuthenticatedUser = nameof(AuthenticatedUser);
            public const string Admin = nameof(Admin);
            public const string RegisteredUser = nameof(RegisteredUser);
            public const string StudentUser = nameof(StudentUser);
            public const string ProfessionUser = nameof(ProfessionUser);


        }
    }
}