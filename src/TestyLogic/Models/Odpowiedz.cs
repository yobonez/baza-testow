using System;
using System.Collections.Generic;

namespace TestyLogic.Models;

public partial class Odpowiedzi
{
    public int IdOdpowiedzi { get; set; }

    public string Tresc { get; set; } = null!;

    public bool CzyPoprawna { get; set; }

    public int IdPytania { get; set; }

    public virtual Pytanie IdPytaniaNavigation { get; set; } = null!;
}
