namespace TestyMAUI.UIModels;

public class PrzedmiotUI : IEquatable<PrzedmiotUI>
{
    public PrzedmiotUI () { }
    public PrzedmiotUI(PrzedmiotUI initializer)
    {
        this.IdPrzedmiotu = initializer.IdPrzedmiotu;
        this.Nazwa = initializer.Nazwa;
    }
    public PrzedmiotUI(int idPrzedmiotu, string nazwa)
    {
        Nazwa = nazwa;
        IdPrzedmiotu = idPrzedmiotu;
    }

    public string Nazwa { get; set; } = null!;

    public int IdPrzedmiotu { get; set; }

    public bool Equals(PrzedmiotUI? other) => other is not null && Nazwa.Equals(other.Nazwa);
}
