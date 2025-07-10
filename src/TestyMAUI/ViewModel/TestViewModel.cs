using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TestyLogic.Models;
using TestyMAUI.Services;
using TestyMAUI.UIModels;

namespace TestyMAUI.ViewModel;

public partial class TestViewModel : ObservableObject
{
    //private readonly TestyDBContext _dbContext;
    private ViewModelLoader _viewModelLoader;

    internal ZestawSearchEntryUI TheTest;
    Stack<PytanieSearchEntryUI> answeredQuestions;

    int questionIndex = 0;

    [ObservableProperty]
    string testName = "";


    [ObservableProperty]
    PytanieSearchEntryUI currentQuestion;

    [ObservableProperty]
    IList<object> wybraneOdpowiedzi;

    public TestViewModel(TestyDBContext dbContext, ViewModelLoader viewModelLoader)
    {
        //_dbContext = dbContext;
        _viewModelLoader = viewModelLoader;
        answeredQuestions = new Stack<PytanieSearchEntryUI>();
        WybraneOdpowiedzi = new ObservableCollection<object>();
    }

    async Task GoToNextQuestion()
    {
        if (CurrentQuestion is not null && !answeredQuestions.Contains(CurrentQuestion)) {
            CurrentQuestion.Odpowiedzi.ForEach(odp =>
                odp.CzyPoprawna = WybraneOdpowiedzi.Contains(odp) ? true : false
            );
            answeredQuestions.Push(CurrentQuestion);
            CurrentQuestion = await _viewModelLoader.LoadFullQuestionFromTest(TheTest.Zestaw.Id, TheTest.pytania.ElementAt(questionIndex).Id);
        }
        // TODO: else jakiś idk nie myśle już i naprawić bo sie pierwsze pytanie powtrza, a po cofnięciu to już kaplica

        if(++questionIndex > TheTest.iloscPytan - 1) {
            //await Shell.Current.GoToAsync($"{nameof(TestResultPage)}", new Dictionary<string, object>()
            //{
            //    //...
            //});
            await Shell.Current.DisplayAlert(TestName, "Koniec (dummy)", "OK");
            return;
        }
    }
    async Task GoToPreviousQuestion()
    {
        questionIndex--;
        PytanieUI questionToLoad = TheTest.pytania.ElementAt(questionIndex);
        CurrentQuestion = answeredQuestions.Last();
        WybraneOdpowiedzi.Clear();
        CurrentQuestion.Odpowiedzi.ForEach(odp =>
        {
            if (odp.CzyPoprawna) WybraneOdpowiedzi.Add(odp);
        });

        answeredQuestions.Pop();
    }

    [RelayCommand]
    async Task NextQuestion()
    {
        await GoToNextQuestion();
    }

    [RelayCommand]
    async Task PreviousQuestion()
    {
        await GoToPreviousQuestion();
    }

    async internal void InitializeTest()
    {
        TestName = $"Test: \"{TheTest.Zestaw.Nazwa}\"";
        CurrentQuestion = await _viewModelLoader.LoadFullQuestionFromTest(TheTest.Zestaw.Id, TheTest.pytania.ElementAt(questionIndex).Id);
    }
}
