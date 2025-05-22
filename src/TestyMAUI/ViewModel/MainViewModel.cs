using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace TestyMAUI.ViewModel;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    bool optionsVisible;

    public MainViewModel()
    {
        OptionsVisible = false;
    }

    [RelayCommand]
    void ToggleOptions()
    {
        OptionsVisible = !OptionsVisible;
    }
}
