namespace TestyLogic.Models;

public partial class PrzynaleznoscPytania
{
    public int IdPytania { get; set; }

    public int IdPrzedmiotu { get; set; }

    public int IdKategorii { get; set; }

    public virtual Kategoria IdKategoriiNavigation { get; set; } = null!;

    public virtual Przedmiot IdPrzedmiotuNavigation { get; set; } = null!;

    public virtual Pytanie IdPytaniaNavigation { get; set; } = null!;
}
