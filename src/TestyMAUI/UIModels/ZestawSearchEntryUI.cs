using CommunityToolkit.Mvvm.ComponentModel;

namespace TestyMAUI.UIModels;

public partial class ZestawSearchEntryUI : ObservableObject
{
    [ObservableProperty]
    public ZestawUI zestaw;
    [ObservableProperty]
    public PrzedmiotUI przedmiot;

    public int iloscPytan;
    public int iloscPunktow;

    [ObservableProperty]
    string iloscPytanText;
    [ObservableProperty]
    string iloscPunktowText;

    public List<PytanieUI>? pytania;

    public ZestawSearchEntryUI(ZestawUI zestaw, PrzedmiotUI przedmiot, List<PytanieUI>? pytania, int? iloscPytan, int? iloscPunktow)
    {
        this.zestaw = zestaw;
        this.przedmiot = przedmiot;
        this.pytania = pytania;
        this.iloscPytan = iloscPytan ?? 0;
        this.iloscPunktow = iloscPunktow ?? 0;

        IloscPunktowText = (iloscPunktow != 0) ? $"Punkty do uzyskania: {iloscPunktow}" : "";
        IloscPytanText = (iloscPytan != 0) ? $"{iloscPytan} pytań" : "";
    }
}
