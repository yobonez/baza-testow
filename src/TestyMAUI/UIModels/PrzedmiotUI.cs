using System;
using System.Collections.Generic;

namespace TestyMAUI.UIModels;

public class PrzedmiotUI
{
    public PrzedmiotUI(int idPrzedmiotu, string nazwa)
    {
        Nazwa = nazwa;
        IdPrzedmiotu = idPrzedmiotu;
    }

    public string Nazwa { get; set; } = null!;

    public int IdPrzedmiotu { get; set; }
}
