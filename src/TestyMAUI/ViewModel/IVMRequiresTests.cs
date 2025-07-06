using System.Collections.ObjectModel;
using TestyMAUI.UIModels;

namespace TestyMAUI.ViewModel;

public interface IVMRequiresTests
{
    ObservableCollection<ZestawUI> Zestawy { get; set; }
}
