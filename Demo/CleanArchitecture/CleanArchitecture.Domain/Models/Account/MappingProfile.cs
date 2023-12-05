using CleanArchitecture.Domain.Constants;
using CleanArchitecture.Domain.Models.Account.DTO;
using CleanArchitecture.Domain.Models.Account.Enum;
using CleanArchitecture.Domain.Models.Account.ValueObjects;

namespace CleanArchitecture.Domain.Models.Account
{
    public class MappingProfile : AutoMapper.Profile
    {
        public MappingProfile()
        {
            CreateMap<AccountEntity, UserResponse>();
            CreateMap<RegisterUserProfileRequest, AccountEntity>()
            .ForMember(x => x.Student, y => y.MapFrom(t => t.Student == null ? new StudentInfo(null, null) : t.Student))
            .ForMember(x => x.Profession, y => y.MapFrom(t => t.Profession == null ? new ProfessionInfo(null, null) : t.Profession))
            .ForMember(x => x.Status, y => y.MapFrom(t => AccountStatus.Registered))
            .ForMember(x => x.Role, y => y.MapFrom(t => Global.AccountRole.User));
        }
    }
}