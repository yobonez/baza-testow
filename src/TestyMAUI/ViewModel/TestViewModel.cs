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

    [ObservableProperty]
    List<PytanieSearchEntryUI> loadedQuestions;

    int questionIndex = 0;

    [ObservableProperty]
    string testName = "";

    [ObservableProperty]
    string questionCounterText = "";

    [ObservableProperty]
    string nextButton = "Dalej >";

    [ObservableProperty]
    bool testCompleted = false;

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


    async Task GoToNextQuestion()
    {
        questionIndex++;

        CurrentQuestion.Odpowiedzi.ForEach(odp =>
            odp.CzyPoprawna = WybraneOdpowiedzi.Contains(odp) ? true : false
        );

        if (questionIndex >= TheTest.iloscPytan)
        {
            questionIndex--;
            TestCompleted = true;

            return;
        }

        UpdateQuestionCounterText();

        if (questionIndex >= LoadedQuestions.Count)
        {   
            CurrentQuestion = await _viewModelLoader.LoadFullQuestionFromTest(TheTest.Zestaw.Id, TheTest.pytania.ElementAt(questionIndex).Id);
            LoadedQuestions.Add(CurrentQuestion!);
        }
        else
            CurrentQuestion = LoadedQuestions.ElementAt(questionIndex);

        RefreshSelectedAnswers();
    }
    async Task GoToPreviousQuestion()
    {
        if (questionIndex == 0) return;
        
        questionIndex--;
        
        CurrentQuestion = LoadedQuestions.ElementAt(questionIndex);

        RefreshSelectedAnswers();

        UpdateQuestionCounterText();
    }

    [RelayCommand]
    async Task NextQuestion() => await GoToNextQuestion();

    [RelayCommand]
    async Task PreviousQuestion() => await GoToPreviousQuestion();

    async internal void InitializeTest()
    {
        TestName = $"Test: \"{TheTest.Zestaw.Nazwa}\"";

        CurrentQuestion = await _viewModelLoader.LoadFullQuestionFromTest(TheTest.Zestaw.Id, TheTest.pytania.ElementAt(questionIndex).Id);
        LoadedQuestions.Add(CurrentQuestion!);

        UpdateQuestionCounterText();    
    }
    void UpdateQuestionCounterText()
    {
        QuestionCounterText = $"Pytanie {questionIndex + 1} z {TheTest.iloscPytan}";
        NextButton = (questionIndex == TheTest.iloscPytan - 1) ? "Zakończ" : "Dalej >";
    }

    void RefreshSelectedAnswers()
    {
        WybraneOdpowiedzi.Clear();
        CurrentQuestion.Odpowiedzi.ForEach(odp =>
        {
            if (odp.CzyPoprawna)
                WybraneOdpowiedzi.Add(odp);
        });
    }

}
