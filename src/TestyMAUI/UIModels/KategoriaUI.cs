namespace TestyMAUI.UIModels;

public class KategoriaUI : IEquatable<KategoriaUI>
{
    public KategoriaUI() { }
    public KategoriaUI(int idKategorii, string nazwa)
    {
        Id = idKategorii;
        Nazwa = nazwa;
    }

    public int Id { get; set; }

    public string Nazwa { get; set; } = null!;

    public bool Equals(KategoriaUI? other) => other is not null && Nazwa.Equals(other.Nazwa);
}
