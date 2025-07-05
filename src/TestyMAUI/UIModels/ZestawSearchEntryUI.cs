namespace TestyMAUI.UIModels;

public class ZestawSearchEntryUI
{
    public ZestawUI zestaw;
    public PrzedmiotUI przedmiot;
    public List<PytanieUI> pytania;

    public ZestawSearchEntryUI(ZestawUI zestaw, PrzedmiotUI przedmiot, List<PytanieUI> pytania)
    {
        this.zestaw = zestaw;
        this.przedmiot = przedmiot;
        this.pytania = pytania;
    }

    public ZestawSearchEntryUI(ZestawSearchEntryUI other)
    {
        zestaw = other.zestaw;
        przedmiot = other.przedmiot;
        pytania = new(other.pytania);
    }
}
