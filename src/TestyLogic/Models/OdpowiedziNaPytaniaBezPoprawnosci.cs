using System;
using System.Collections.Generic;

namespace TestyLogic.Models;

public partial class OdpowiedziNaPytaniaBezPoprawnosci
{
    public int IdPytania { get; set; }

    public string Pytanie { get; set; } = null!;

    public short Punkty { get; set; }

    public int IdOdpowiedzi { get; set; }

    public string Odpowiedz { get; set; } = null!;
}
