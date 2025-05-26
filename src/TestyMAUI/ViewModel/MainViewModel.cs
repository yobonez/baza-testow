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

    [RelayCommand]
    async Task TapQuestionsCreator()
    {
        await Shell.Current.GoToAsync(nameof(QuestionsCreatorPage));
    }
    [RelayCommand]
    async Task TapTestsCreator()
    {
        await Shell.Current.GoToAsync(nameof(TestsCreatorPage));
    }
    [RelayCommand]
    async Task TapTestSelector()
    {
        await Shell.Current.GoToAsync(nameof(TestSelectorPage));
    }
}
