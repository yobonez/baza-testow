namespace TestyLogic.Models;

public partial class Odpowiedz
{
    public int IdOdpowiedzi { get; set; }

    public string Tresc { get; set; } = null!;

    public bool CzyPoprawna { get; set; }

    public int IdPytania { get; set; }

    public virtual Pytanie IdPytaniaNavigation { get; set; } = null!;
}
