using AutoMapper;
using WebApplication1.DataAccess.Entities;
using WebApplication1.Infrastructure.Models;

namespace WebApplication1.Infrastructure
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<PostTranslation, PostModel>()
                .ForMember(dest => dest.Published, opt => opt.MapFrom(src => src.Post.Published))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.Post.CreatedDate))
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => src.Post.UpdatedDate))
                .ReverseMap()
                .ForMember(x => x.Id, opt => opt.Ignore())
                .AfterMap((src, dest) => dest.Post.Published = src.Published);
            CreateMap<PostModel, Post>()
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.CreatedDate, opt => opt.Ignore())
                .ForMember(x => x.UpdatedDate, opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    dest.PostTranslations.Add(new PostTranslation()
                    {
                        LanguageCode = src.LanguageCode,
                        Title = src.Title,
                        Description = src.Description
                    });
                });
        }
    }
}
