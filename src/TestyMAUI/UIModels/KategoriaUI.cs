namespace TestyMAUI.UIModels;

public class KategoriaUI : IEquatable<KategoriaUI>
{
    public KategoriaUI() { }
    public KategoriaUI(int idKategorii, string nazwa)
    {
        IdKategorii = idKategorii;
        Nazwa = nazwa;
    }

    public int IdKategorii { get; set; }

    public string Nazwa { get; set; } = null!;

    public bool Equals(KategoriaUI? other) => other is not null && Nazwa.Equals(other.Nazwa);
}
