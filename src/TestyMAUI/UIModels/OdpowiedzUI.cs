using CommunityToolkit.Mvvm.ComponentModel;

namespace TestyMAUI.UIModels;

public partial class OdpowiedzUI : ObservableObject, IIndexable
{
    public OdpowiedzUI() { }
    public OdpowiedzUI(int idOdpowiedzi, string tresc, bool czyPoprawna, int idPytania)
    {
        Id = idOdpowiedzi;
        Tresc = tresc;
        CzyPoprawna = czyPoprawna;
        IdPytania = idPytania;
    }

    public int Id { get; set; }

    public string Tresc { get; set; } = null!;
    public bool CzyPoprawna { get; set; }

    public int IdPytania { get; set; }

    [ObservableProperty]
    Color selectedResultBkgColor = new Color(0, 0, 0, 0); 

    [ObservableProperty]
    public ImageSource correctnessIcon = null!;

    [ObservableProperty]
    public string idx;
}
