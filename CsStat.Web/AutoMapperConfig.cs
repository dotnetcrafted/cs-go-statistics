using AutoMapper;
using CsStat.Domain.Entities;
using CsStat.Web.Models;

namespace CsStat.Web
{
    public class AutoMapperConfig
    {
        public class PlayerProfile : Profile
        {
            public PlayerProfile()
            {
                CreateMap<Player, PlayerModelDto>()
                    .ForMember(dest => dest.NickName, opts => opts.MapFrom(src => src.NickName))
                    .ForMember(dest => dest.FirstName, opts => opts.MapFrom(src => src.FirstName))
                    .ForMember(dest => dest.SecondName, opts => opts.MapFrom(src => src.SecondName))
                    .ForMember(dest => dest.ImagePath, opts => opts.MapFrom(src => src.ImagePath));
            }
        }
    }
}