namespace TestyMAUI.UIModels;

public class OdpowiedzUI
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


}
