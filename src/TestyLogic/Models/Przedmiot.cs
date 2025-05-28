namespace TestyLogic.Models;

public partial class Przedmiot
{
    public string Nazwa { get; set; } = null!;

    public int IdPrzedmiotu { get; set; }

    public virtual ICollection<PrzynaleznoscPytania> PrzynaleznoscPytanNavigation { get; set; } = new List<PrzynaleznoscPytania>();

    public virtual ICollection<PytanieWZestawie> PytaniaWZestawachNavigation { get; set; } = new List<PytanieWZestawie>();
}
