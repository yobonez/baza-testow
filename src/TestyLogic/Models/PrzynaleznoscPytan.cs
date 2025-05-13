using System;
using System.Collections.Generic;

namespace TestyLogic.Models;

public partial class PrzynaleznoscPytan
{
    public int IdPytania { get; set; }

    public int IdPrzedmiotu { get; set; }

    public int IdKategorii { get; set; }

    public virtual Kategorie IdKategoriiNavigation { get; set; } = null!;

    public virtual Przedmioty IdPrzedmiotuNavigation { get; set; } = null!;

    public virtual Pytania IdPytaniaNavigation { get; set; } = null!;
}
