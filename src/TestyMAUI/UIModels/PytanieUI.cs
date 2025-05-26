using System;
using System.Collections.Generic;

namespace TestyMAUI.UIModels;

public class PytanieUI
{
    public PytanieUI() { }
    public PytanieUI(int id, string tresc, short pkt, bool typ)
    {
        IdPytania = id;
        Tresc = tresc;
        Punkty = pkt;
        TypPytania = typ;
    }

    public int IdPytania { get; set; }

    public string Tresc { get; set; } = null!;

    public short Punkty { get; set; }

    public bool TypPytania { get; set; }
}
