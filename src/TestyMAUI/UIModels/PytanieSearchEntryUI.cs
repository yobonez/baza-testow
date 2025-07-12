using CommunityToolkit.Mvvm.ComponentModel;

namespace TestyMAUI.UIModels;

public partial class PytanieSearchEntryUI : ObservableObject
{
    [ObservableProperty]
    public PytanieUI pytanie;
    [ObservableProperty]
    public PrzedmiotUI przedmiot;
    [ObservableProperty]
    public KategoriaUI? kategoria;
    [ObservableProperty]
    public List<OdpowiedzUI>? odpowiedzi;

    [ObservableProperty]
    public IList<object>? chosenAnswers;

    public PytanieSearchEntryUI(PytanieUI pyt, PrzedmiotUI prz, KategoriaUI? kat = null, List<OdpowiedzUI>? odp = null)
    {
        pytanie = pyt;
        przedmiot = prz;
        kategoria = kat;
        odpowiedzi = odp;
    }
}
