namespace TestyLogic.Models;

public partial class Kategoria
{
    public int IdKategorii { get; set; }

    public string Nazwa { get; set; } = null!;

    public virtual ICollection<PrzynaleznoscPytania> PrzynaleznoscPytanNavigation { get; set; } = new List<PrzynaleznoscPytania>();
}
