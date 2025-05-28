namespace TestyMAUI.UIModels;

public class PytanieUI
{
    public PytanieUI() { }
    public PytanieUI(int id, string tresc, short pkt, bool typ, PrzedmiotUI? przedmiot, KategoriaUI kategoria, List<OdpowiedzUI> odpowiedzi)
    {
        IdPytania = id;
        Tresc = tresc;
        Punkty = pkt;
        TypPytania = typ;
        Przedmiot = przedmiot;
        Kategoria = kategoria;
        Odpowiedzi = odpowiedzi;
    }

    public int IdPytania { get; set; }

    public string Tresc { get; set; } = null!;

    public short Punkty { get; set; }

    public bool TypPytania { get; set; }
    public PrzedmiotUI? Przedmiot { get; set; }
    public KategoriaUI? Kategoria { get; set; }
    public List<OdpowiedzUI>? Odpowiedzi { get; set; }
}
