using System;
using System.Collections.Generic;

namespace TestyLogic.Models;

public partial class Zestawy
{
    public int IdZestawu { get; set; }

    public string Nazwa { get; set; } = null!;

    public DateTime DataUtworzenia { get; set; }

    public int IdPrzedmiotu { get; set; }

    public virtual ICollection<PytaniaWZestawach> PytaniaWZestawachNavigation { get; set; } = new List<PytaniaWZestawach>();
}
