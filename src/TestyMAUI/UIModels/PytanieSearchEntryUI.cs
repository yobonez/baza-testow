using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestyMAUI.UIModels
{
    public class PytanieSearchEntryUI
    {
        public PytanieUI pytanie;
        public PrzedmiotUI przedmiot;
        public KategoriaUI kategoria;
        public List<OdpowiedzUI> odpowiedzi;

        public PytanieSearchEntryUI(PytanieUI pyt, PrzedmiotUI prz, KategoriaUI kat, List<OdpowiedzUI> odp)
        {
            pytanie = pyt;
            przedmiot = prz;
            kategoria = kat;
            odpowiedzi = odp;
        }
    }
}
