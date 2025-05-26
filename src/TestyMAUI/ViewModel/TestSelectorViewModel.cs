using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestyMAUI.UIModels;

namespace TestyMAUI.ViewModel
{
    public partial class TestSelectorViewModel : ObservableObject
    {
        public TestSelectorViewModel()
        {
            Zestawy = new() { 
                new ZestawUI(0, "Testowy zestaw", DateTime.Now, 0) ,
                new ZestawUI(0, "Testowy zestaw2", DateTime.Now, 0),
                new ZestawUI(0, "Testowy zestaw3", DateTime.Now, 0),
                new ZestawUI(0, "Testowy zestaw4", DateTime.Now, 0),
                new ZestawUI(0, "Testowy zestaw5", DateTime.Now, 0),
            };
        }

        [ObservableProperty]
        ObservableCollection<ZestawUI> zestawy;
    }
}
