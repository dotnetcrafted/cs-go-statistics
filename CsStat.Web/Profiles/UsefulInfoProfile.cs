using AutoMapper;
using CsStat.Domain.Entities;
using CsStat.Web.Models;

namespace CsStat.Web.Profiles
{
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
                .ForMember(dest => dest.Tags, opts => opts.MapFrom(src => string.Join(";", src.Tags)))
                .ForMember(dest => dest.Image, opts => opts.MapFrom(src => src.ImagePath))
                .ForAllOtherMembers(x => x.Ignore())
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