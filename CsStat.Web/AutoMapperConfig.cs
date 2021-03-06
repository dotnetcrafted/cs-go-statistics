﻿using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CsStat.Domain;
using CsStat.Domain.Entities;
using CsStat.Domain.Models;
using CsStat.Web.Models;

namespace CsStat.Web
{
    public class AutoMapperConfig
    {
        public class PlayerStatProfile : Profile
        {
            public PlayerStatProfile()
            {
                CreateMap<PlayerStatsModel, PlayerStatsViewModel>()
                    .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Player.Id))
                    .ForMember(dest => dest.SteamId, opts => opts.MapFrom(src => src.Player.SteamId))
                    .ForMember(dest => dest.Assists, opts => opts.MapFrom(src => src.Assists))
                    .ForMember(dest => dest.AssistsPerGame, opts => opts.MapFrom(src => src.AssistsPerGame))
                    .ForMember(dest => dest.Deaths, opts => opts.MapFrom(src => src.Deaths))
                    .ForMember(dest => dest.DeathsPerGame, opts => opts.MapFrom(src => src.DeathPerGame))
                    .ForMember(dest => dest.DefusedBombs, opts => opts.MapFrom(src => src.Defuse))
                    .ForMember(dest => dest.ExplodedBombs, opts => opts.MapFrom(src => src.Explode))
                    .ForMember(dest => dest.FriendlyKills, opts => opts.MapFrom(src => src.FriendlyKills))
                    .ForMember(dest => dest.KdRatio, opts => opts.MapFrom(src => src.KdRatio))
                    .ForMember(dest => dest.Kills, opts => opts.MapFrom(src => src.Kills))
                    .ForMember(dest => dest.KillsPerGame, opts => opts.MapFrom(src => src.KillsPerGame))
                    .ForMember(dest => dest.Points, opts => opts.MapFrom(src => src.Points))
                    .ForMember(dest => dest.TotalGames, opts => opts.MapFrom(src => src.TotalGames))
                    .ForMember(dest => dest.HeadShot, opts => opts.MapFrom(src => src.HeadShotsCount))
                    .ForMember(dest => dest.KdDif, opts => opts.MapFrom(src => src.KdDif))
                    .AfterMap((s, d, context) =>
                    {
                        if (s.Guns != null && s.Guns.Any())
                        {
                            d.Guns = context.Mapper.Map<List<WeaponViewModel>>(s.Guns);
                        }

                        if (d.Victims != null && s.Victims.Any())
                        {
                            d.Victims = s.Victims;
                        }

                        if (d.Killers != null && s.Killers.Any())
                        {
                            d.Killers = s.Killers;
                        }

                        if (d.Achievements != null && d.Achievements.Any())
                        {
                            d.Achievements = context.Mapper.Map<List<AchievementViewModel>>(s.Achievements);
                            foreach (var achievement in d.Achievements)
                            {
                                achievement.IconUrl = $"{Settings.AdminPath}{achievement.IconUrl}";
                            }
                        }
                    });

                CreateMap<WeaponStatModel, WeaponViewModel>()
                    .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Weapon.Id))
                    .ForMember(dest => dest.Name, opts => opts.MapFrom(src => src.Weapon.Name))
                    .ForMember(dest => dest.IconImage, opts => opts.MapFrom(src => src.Weapon.Icon.FullUrl))
                    .ForMember(dest => dest.PhotoImage, opts => opts.MapFrom(src => src.Weapon.Image.FullUrl))
                    .ForMember(dest => dest.Type, opts => opts.MapFrom(src => src.Weapon.Type.Name))
                    .AfterMap((s, d, context) => { d.Kills = s.Kills; });

                CreateMap<AchieveModel, AchievementViewModel>()
                    .ForMember(dest => dest.IconUrl, opts => opts.MapFrom(src => src.Icon.Url))
                    .ForMember(dest => dest.AchievementId, opts => opts.MapFrom(src => src.AchievementId))
                    .ForMember(dest => dest.Name, opts => opts.MapFrom(src => src.Name))
                    .ForMember(dest => dest.Description, opts => opts.MapFrom(src => src.Description))
                    ;
            }



        }

        public class UsefulInfoProfile : Profile
        {
            public UsefulInfoProfile()
            {
                CreateMap<InfoViewModel, UsefulInfo>()
                    .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
                    .ForMember(dest => dest.Caption, opts => opts.MapFrom(src => src.Caption))
                    .ForMember(dest => dest.Description, opts => opts.MapFrom(src => src.Description))
                    .ForMember(dest => dest.PublishDate, opts => opts.MapFrom(src => src.PublishDate))
                    .ForMember(dest => dest.Url, opts => opts.MapFrom(src => src.Url))
                    .ForMember(dest => dest.Tags, opts => opts.MapFrom(src => string.Join(";",src.Tags)))
                    .ForMember(dest => dest.Image, opts => opts.MapFrom(src => src.ImagePath))
                    .ForAllOtherMembers(x=>x.Ignore())
                    ;

                CreateMap<UsefulInfo, InfoViewModel>()
                    .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
                    .ForMember(dest => dest.Caption, opts => opts.MapFrom(src => src.Caption))
                    .ForMember(dest => dest.Description, opts => opts.MapFrom(src => src.Description))
                    .ForMember(dest => dest.PublishDate, opts => opts.MapFrom(src => src.PublishDate))
                    .ForMember(dest => dest.Url, opts => opts.MapFrom(src => src.Url))
                    .ForMember(dest => dest.ImagePath, opts => opts.MapFrom(src => src.Image))
                    .ForMember(dest => dest.Tags, opts => opts.MapFrom(src => src.Tags.Split(';')))
                    .ForAllOtherMembers(x => x.Ignore())
                    ;
            }
        }

    }
}