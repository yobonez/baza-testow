using System;
using System.Collections.Generic;

namespace TestyLogic.Models;

public partial class Kategorie
{
    public int IdKategorii { get; set; }

    public string Nazwa { get; set; } = null!;

    public virtual ICollection<PrzynaleznoscPytan> PrzynaleznoscPytanNavigation { get; set; } = new List<PrzynaleznoscPytan>();
}
