using AutoMapper;
using CsStat.Domain.Models;
using CsStat.Web.Models;

namespace CsStat.Web.Profiles
{
    public class AchievementsProfile : Profile
    {
        public AchievementsProfile()
        {
            CreateMap<AchieveModel, AchievementViewModel>()
                .ForMember(dest => dest.IconUrl, opts => opts.MapFrom(src => src.Icon.Url))
                .ForMember(dest => dest.AchievementId, opts => opts.MapFrom(src => src.AchievementId))
                .ForMember(dest => dest.Name, opts => opts.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opts => opts.MapFrom(src => src.Description))
                ;
        }
    }
}