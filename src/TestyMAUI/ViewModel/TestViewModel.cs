using CommunityToolkit.Mvvm.ComponentModel;
using TestyLogic.Models;
using TestyMAUI.Services;
using TestyMAUI.UIModels;

namespace TestyMAUI.ViewModel;

public partial class TestViewModel : ObservableObject
{
    private readonly TestyDBContext _dbContext;
    private ViewModelLoader _viewModelLoader;

    internal ZestawSearchEntryUI TheTest;

    [ObservableProperty]
    string testName = "";
    [ObservableProperty]
    PytanieSearchEntryUI currentQuestion;

    ZestawSearchEntryUI TheAnsweredTest;

    public TestViewModel(TestyDBContext dbContext, ViewModelLoader viewModelLoader)
    {
        _dbContext = dbContext;
        _viewModelLoader = viewModelLoader;
    }

    async internal void InitializeTest()
    {
        TestName = TheTest.Zestaw.Nazwa;
        CurrentQuestion = await _viewModelLoader.LoadFullQuestionFromTest(TheTest.Zestaw.Id, TheTest.pytania.First().Id);
    }
}
