using CommunityToolkit.Mvvm.ComponentModel;

namespace TestyMAUI.UIModels;

public partial class PytanieUI : ObservableObject
{
    public PytanieUI() { }
    public PytanieUI(int id, string tresc, short pkt, bool typ)
    {
        Id = id;
        Tresc = tresc;
        Punkty = pkt;
        TypPytania = typ;
    }

    public int Id { get; set; }

    public string Tresc { get; set; } = null!;

    public short Punkty { get; set; }

    public bool TypPytania { get; set; }

    [ObservableProperty]
    public string idx;
}
