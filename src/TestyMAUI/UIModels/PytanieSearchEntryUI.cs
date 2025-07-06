namespace TestyMAUI.UIModels;

public class PytanieSearchEntryUI
{
    public PytanieUI pytanie;
    public PrzedmiotUI przedmiot;
    public KategoriaUI? kategoria;
    public List<OdpowiedzUI>? odpowiedzi;

    public PytanieSearchEntryUI(PytanieUI pyt, PrzedmiotUI prz, KategoriaUI? kat = null, List<OdpowiedzUI>? odp = null)
    {
        pytanie = pyt;
        przedmiot = prz;
        kategoria = kat;
        odpowiedzi = odp;
    }
}
