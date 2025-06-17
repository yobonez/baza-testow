namespace TestyMAUI.UIModels;

public class PrzedmiotUI : IEquatable<PrzedmiotUI>
{
    public PrzedmiotUI () { }
    public PrzedmiotUI(int idPrzedmiotu, string nazwa)
    {
        Id = idPrzedmiotu;
        Nazwa = nazwa;
    }

    public int Id { get; set; }

    public string Nazwa { get; set; } = null!;

    public bool Equals(PrzedmiotUI? other) => other is not null && Nazwa.Equals(other.Nazwa);
}
