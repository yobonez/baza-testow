namespace TestyLogic.Models;

public partial class PytanieWZestawie
{
    public int IdPytania { get; set; }

    public int IdZestawu { get; set; }

    public int IdPrzedmiotu { get; set; }

    public virtual Przedmiot IdPrzedmiotuNavigation { get; set; } = null!;

    public virtual Pytanie IdPytaniaNavigation { get; set; } = null!;

    public virtual Zestaw IdZestawuNavigation { get; set; } = null!;
}
