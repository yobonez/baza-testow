using CommunityToolkit.Mvvm.ComponentModel;

namespace TestyMAUI.UIModels;

public partial class ZestawSearchEntryUI : ObservableObject
{
    [ObservableProperty]
    public ZestawUI zestaw;
    [ObservableProperty]
    public PrzedmiotUI przedmiot;
    public List<PytanieUI>? pytania;

    public ZestawSearchEntryUI(ZestawUI zestaw, PrzedmiotUI przedmiot, List<PytanieUI>? pytania)
    {
        this.zestaw = zestaw;
        this.przedmiot = przedmiot;
        this.pytania = pytania;
    }
}
