using System;
using System.Collections.Generic;

namespace TestyLogic.Models;

public partial class Przedmioty
{
    public string Nazwa { get; set; } = null!;

    public int IdPrzedmiotu { get; set; }

    public virtual ICollection<PrzynaleznoscPytan> PrzynaleznoscPytanNavigation { get; set; } = new List<PrzynaleznoscPytan>();

    public virtual ICollection<PytaniaWZestawach> PytaniaWZestawachNavigation { get; set; } = new List<PytaniaWZestawach>();
}
