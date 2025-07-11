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
    List<PytanieSearchEntryUI> loadedQuestions;

    int questionIndex = 0;


    [ObservableProperty]
    string testName = "";

    [ObservableProperty]
    string questionCounterText = "";

    [ObservableProperty]
    PytanieSearchEntryUI currentQuestion;

    [ObservableProperty]
    IList<object> wybraneOdpowiedzi;

    public TestViewModel(TestyDBContext dbContext, ViewModelLoader viewModelLoader)
    {
        //_dbContext = dbContext;
        _viewModelLoader = viewModelLoader;
        loadedQuestions = new List<PytanieSearchEntryUI>();
        WybraneOdpowiedzi = new ObservableCollection<object>();
    }

    void UpdateQuestionCounterText() => QuestionCounterText = $"Pytanie {questionIndex + 1} z {TheTest.iloscPytan}";

    // TODO: zapisuje sie tylko jedno pytanie pls fix
    async Task GoToNextQuestion()
    {
        questionIndex++;

        CurrentQuestion.Odpowiedzi.ForEach(odp =>
            odp.CzyPoprawna = WybraneOdpowiedzi.Contains(odp) ? true : false
        );

        if (questionIndex > TheTest.iloscPytan)
        {
            await Shell.Current.DisplayAlert(TestName, "Koniec (dummy)", "OK");
            return;
        }

        UpdateQuestionCounterText();
        if (questionIndex >= loadedQuestions.Count)
        {
            CurrentQuestion = await _viewModelLoader.LoadFullQuestionFromTest(TheTest.Zestaw.Id, TheTest.pytania.ElementAt(questionIndex).Id);
            loadedQuestions.Add(CurrentQuestion!);
        }
        else
            CurrentQuestion = loadedQuestions.ElementAt(questionIndex);
    }
    async Task GoToPreviousQuestion()
    {
        if (questionIndex == 0) return;
        
        questionIndex--;
        
        CurrentQuestion = loadedQuestions.ElementAt(questionIndex);
        UpdateQuestionCounterText();

        List<OdpowiedzUI> castedWybraneOdpowiedzi = WybraneOdpowiedzi.Cast<OdpowiedzUI>().ToList();

        castedWybraneOdpowiedzi.ForEach(odp =>
            odp.CzyPoprawna = CurrentQuestion.Odpowiedzi.Where(popodp => popodp.CzyPoprawna).Contains(odp) ? true : false
        );
    }

    [RelayCommand]
    async Task NextQuestion() => await GoToNextQuestion();

    [RelayCommand]
    async Task PreviousQuestion() => await GoToPreviousQuestion();

    async internal void InitializeTest()
    {
        TestName = $"Test: \"{TheTest.Zestaw.Nazwa}\"";
        CurrentQuestion = await _viewModelLoader.LoadFullQuestionFromTest(TheTest.Zestaw.Id, TheTest.pytania.ElementAt(questionIndex).Id);
        loadedQuestions.Add(CurrentQuestion!);
        UpdateQuestionCounterText();
    }
}
