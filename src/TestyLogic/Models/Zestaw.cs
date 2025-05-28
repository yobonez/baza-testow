namespace TestyLogic.Models;

public partial class Zestaw
{
    public int IdZestawu { get; set; }

    public string Nazwa { get; set; } = null!;

    public DateTime DataUtworzenia { get; set; }

    public int IdPrzedmiotu { get; set; }

    public virtual ICollection<PytanieWZestawie> PytaniaWZestawachNavigation { get; set; } = new List<PytanieWZestawie>();
}
