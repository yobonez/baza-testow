using AutoMapper;
using TestyLogic.Models;
using TestyMAUI.UIModels;

namespace TestyMAUI.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() {
            CreateMap<Przedmiot, PrzedmiotUI>()
                .ForMember(dest => dest.Id,
                           prop => prop.MapFrom(src => src.IdPrzedmiotu))
                .ForMember(dest => dest.Nazwa,
                           prop => prop.MapFrom(src => src.Nazwa));

            CreateMap<Kategoria, KategoriaUI>()
                .ForMember(dest => dest.Id,
                           prop => prop.MapFrom(src => src.IdKategorii))
                .ForMember(dest => dest.Nazwa,
                           prop => prop.MapFrom(src => src.Nazwa));
        }
    }
}
