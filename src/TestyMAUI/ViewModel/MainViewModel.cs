using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TestyLogic.Models;

namespace TestyMAUI.ViewModel;

public partial class MainViewModel : ObservableObject
{
    private readonly TestyDBContext _dbContext;

    [ObservableProperty]
    bool optionsVisible;

    [ObservableProperty]
    ImageSource statusImage;

    public MainViewModel(TestyDBContext dbContext)
    {
        OptionsVisible = false;
        _dbContext = dbContext;
        StatusImage = "db_connection_wait.png";
        StatusImage = (dbContext.Database.CanConnect() ?
            "db_connection_success.png" : "db_connection_fail.png");
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
