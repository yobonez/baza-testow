using System;
using System.Collections.Generic;

namespace TestyMAUI.UIModels;

public class Pytanie
{
    public int IdPytania { get; set; }

    public string Tresc { get; set; } = null!;

    public short Punkty { get; set; }

    public bool TypPytania { get; set; }
}
