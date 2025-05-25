using System;
using System.Collections.Generic;

namespace TestyMAUI.UIModels;

public class KategoriaUI
{
    public KategoriaUI(int idKategorii, string nazwa)
    {
        IdKategorii = idKategorii;
        Nazwa = nazwa;
    }

    public int IdKategorii { get; set; }

    public string Nazwa { get; set; } = null!;
}
