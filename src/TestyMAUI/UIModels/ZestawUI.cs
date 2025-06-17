namespace TestyMAUI.UIModels;

public class ZestawUI
{
    public ZestawUI()
    {
    }

    public ZestawUI(int idZestawu, string nazwa, DateTime dataUtworzenia, int idPrzedmiotu)
    {
        Id = idZestawu;
        Nazwa = nazwa;
        DataUtworzenia = dataUtworzenia;
        IdPrzedmiotu = idPrzedmiotu;
    }

    public int Id { get; set; }

    public string Nazwa { get; set; } = null!;

    public DateTime DataUtworzenia { get; set; }

    public int IdPrzedmiotu { get; set; }
    public PrzedmiotUI? Przedmiot { get; set; }
    public KategoriaUI? Kategoria { get; set; }
    public List<PytanieUI>? Pytania { get; set; }
}
