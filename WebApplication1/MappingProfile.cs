using System;
using AutoMapper;
using WebApplication1.Enums;
using WebApplication1.Models;

namespace WebApplication1
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<PostTranslationCreate, PostTranslation>()
                .ForMember(dest => dest.LanguageCode, 
                    opt => opt.MapFrom(src => (Enum.IsDefined(typeof(LanguageCode), src.LanguageCode) ? src.LanguageCode : LanguageCode.en).ToString()))
                .ReverseMap();
            CreateMap<PostTranslationUpdate, PostTranslation>()
                .ForMember(dest => dest.LanguageCode,
                    opt => opt.MapFrom(src => (Enum.IsDefined(typeof(LanguageCode), src.LanguageCode) ? src.LanguageCode : LanguageCode.en).ToString()))
                .ReverseMap();
        }
    }
}
