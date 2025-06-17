using AutoMapper;
using TestyLogic.Models;
using TestyMAUI.UIModels;

namespace TestyMAUI.Mapper;

public class DtoToUI : Profile
{
    public DtoToUI() {
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
        CreateMap<Pytanie, PytanieUI>()
            .ForMember(dest => dest.Id,
                       prop => prop.MapFrom(src => src.IdPytania))
            .ForMember(dest => dest.Tresc,
                       prop => prop.MapFrom(src => src.Tresc))
            .ForMember(dest => dest.Punkty,
                       prop => prop.MapFrom(src => src.Punkty))
            .ForMember(dest => dest.TypPytania,
                       prop => prop.MapFrom(src => src.TypPytania));
        CreateMap<Odpowiedz, OdpowiedzUI>()
            .ForMember(dest => dest.Id,
                       prop => prop.MapFrom(src => src.IdOdpowiedzi))
            .ForMember(dest => dest.Tresc,
                       prop => prop.MapFrom(src => src.Tresc))
            .ForMember(dest => dest.CzyPoprawna,
                       prop => prop.MapFrom(src => src.CzyPoprawna))
            .ForMember(dest => dest.IdPytania,
                       prop => prop.MapFrom(src => src.IdPytania));
    }
}
