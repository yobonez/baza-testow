using CommunityToolkit.Mvvm.ComponentModel;

namespace TestyMAUI.UIModels;

public partial class OdpowiedzUI : ObservableObject
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
    public ImageSource correctnessIcon = null!;
}
