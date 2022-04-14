using System;
using AutoMapper;
using CsStat.Domain.Models;
using CsStat.Web.Models;

namespace CsStat.Web.Profiles
{
    public class WeaponProfile : Profile
    {
        public WeaponProfile()
        {
            CreateMap<WeaponStatModel, WeaponViewModel>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Weapon.Id))
                .ForMember(dest => dest.Name, opts => opts.MapFrom(src => src.Weapon.Name))
                .ForMember(dest => dest.IconImage, opts => opts.MapFrom(src => src.Weapon.Icon.FullUrl))
                .ForMember(dest => dest.PhotoImage, opts => opts.MapFrom(src => src.Weapon.Image.FullUrl))
                .ForMember(dest => dest.Type, opts => opts.MapFrom(src => src.Weapon.Type.Name))
                .AfterMap((s, d, context) => { d.Kills = s.Kills; });

            CreateMap<WeaponStatDto, WeaponsStatsViewModel>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.Kills, opts => opts.MapFrom(src => src.Kills))
                .ForMember(dest => dest.HeadShots, opts => opts.MapFrom(src => src.HeadShots))
                .ForMember(dest => dest.HeadShotsRatio, opts => opts.MapFrom(src => src.Kills != 0 ? (int)(Math.Round((double)src.HeadShots / src.Kills * 100, 0)) : 0))
                ;
        }
    }
}