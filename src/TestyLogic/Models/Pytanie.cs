using System;
using System.Collections.Generic;

namespace TestyLogic.Models;

public partial class Pytanie
{
    public int IdPytania { get; set; }

    public string Tresc { get; set; } = null!;

    public short Punkty { get; set; }

    public bool TypPytania { get; set; }

    public virtual ICollection<Odpowiedz> Odpowiedzi { get; set; } = new List<Odpowiedz>();

    public virtual ICollection<PrzynaleznoscPytania> PrzynaleznoscPytanNavigation { get; set; } = new List<PrzynaleznoscPytania>();

    public virtual ICollection<PytanieWZestawie> PytaniaWZestawachNavigation { get; set; } = new List<PytanieWZestawie>();
}
