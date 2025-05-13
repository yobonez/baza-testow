using System;
using System.Collections.Generic;

namespace TestyLogic.Models;

public partial class Pytania
{
    public int IdPytania { get; set; }

    public string Tresc { get; set; } = null!;

    public short Punkty { get; set; }

    public bool TypPytania { get; set; }

    public virtual ICollection<Odpowiedzi> Odpowiedzis { get; set; } = new List<Odpowiedzi>();

    public virtual ICollection<PrzynaleznoscPytan> PrzynaleznoscPytanNavigation { get; set; } = new List<PrzynaleznoscPytan>();

    public virtual ICollection<PytaniaWZestawach> PytaniaWZestawachNavigation { get; set; } = new List<PytaniaWZestawach>();
}
