using AutoMapper;
using TestyLogic.Models;
using TestyMAUI.UIModels;

namespace TestyMAUI.Mapper;

public class UIToDto : Profile
{
    public UIToDto ()
    {
        CreateMap<PytanieUI, Pytanie>()
            .ForMember(dest => dest.IdPytania,
                       prop => prop.MapFrom(src => src.Id))
            .ForMember(dest => dest.Tresc,
                       prop => prop.MapFrom(src => src.Tresc))
            .ForMember(dest => dest.Punkty,
                       prop => prop.MapFrom(src => src.Punkty))
            .ForMember(dest => dest.TypPytania,
                       prop => prop.MapFrom(src => src.TypPytania))
            .ForMember(dest => dest.Odpowiedzi,
                       prop => prop.Ignore());
        CreateMap<OdpowiedzUI, Odpowiedz>()
            .ForMember(dest => dest.IdOdpowiedzi,
                       prop => prop.MapFrom(src => src.Id))
            .ForMember(dest => dest.Tresc,
                       prop => prop.MapFrom(src => src.Tresc))
            .ForMember(dest => dest.CzyPoprawna,
                       prop => prop.MapFrom(src => src.CzyPoprawna))
            .ForMember(dest => dest.IdPytania,
                       prop => prop.MapFrom(src => src.IdPytania));
        CreateMap<PrzedmiotUI, Przedmiot>()
            .ForMember(dest => dest.IdPrzedmiotu,
                       prop => prop.MapFrom(src => src.Id))
            .ForMember(dest => dest.Nazwa,
                       prop => prop.MapFrom(src => src.Nazwa));
        CreateMap<KategoriaUI, Kategoria>()
            .ForMember(dest => dest.IdKategorii,
                       prop => prop.MapFrom(src => src.Id))
            .ForMember(dest => dest.Nazwa,
                       prop => prop.MapFrom(src => src.Nazwa));



    }
}
