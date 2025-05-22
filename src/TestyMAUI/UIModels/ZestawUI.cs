using System;
using System.Collections.Generic;

namespace TestyMAUI.UIModels;

public class Zestaw
{
    public int IdZestawu { get; set; }

    public string Nazwa { get; set; } = null!;

    public DateTime DataUtworzenia { get; set; }

    public int IdPrzedmiotu { get; set; }
}
