using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CsStat.Domain;
using CsStat.Domain.Entities;
using CsStat.Web.Models;

namespace CsStat.Web.Profiles
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

                    if (d.Achievements == null || !d.Achievements.Any()) 
                        return;

                    d.Achievements = context.Mapper.Map<List<AchievementViewModel>>(s.Achievements);
                    foreach (var achievement in d.Achievements)
                    {
                        achievement.IconUrl = $"{Settings.AdminPath}{achievement.IconUrl}";
                    }
                });
        }
    }
}