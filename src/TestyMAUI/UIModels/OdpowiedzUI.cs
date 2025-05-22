using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestyMAUI.UIModels;

public class OdpowiedzUI
{
    public int IdOdpowiedzi { get; set; }

    public string Tresc { get; set; } = null!;

    public bool CzyPoprawna { get; set; }

    public int IdPytania { get; set; }
}
