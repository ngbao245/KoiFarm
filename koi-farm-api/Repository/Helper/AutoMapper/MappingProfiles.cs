using AutoMapper;
using Repository.Data.Entity;
using Repository.Model.Auth;
using Repository.Model.User;

namespace Repository.Helper.AutoMapper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            // Mapping between User and authentication models
            CreateMap<User, SignInModel>().ReverseMap();
            CreateMap<User, SignUpModel>().ReverseMap();

            // Mapping between User and response models
            CreateMap<User, ResponseUserModel>().ReverseMap();

            // You can also map specific fields in case the properties differ
            CreateMap<UserRefreshToken, ResponseTokenModel>()
                .ForMember(dest => dest.Token, opt => opt.MapFrom(src => src.JwtId))
                .ForMember(dest => dest.RefreshToken, opt => opt.MapFrom(src => src.RefreshToken))
                .ReverseMap();
        }
    }
}
