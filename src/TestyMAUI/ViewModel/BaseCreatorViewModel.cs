using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TestyLogic.Models;
using TestyMAUI.UIModels;

namespace TestyMAUI.ViewModel;

public abstract partial class BaseCreatorViewModel : ObservableObject
{
    protected enum CreatorMessageType
    {
        QuestionMessageFromQuestions,
        QuestionMessageFromTests,
        TestMessage
    }

    [ObservableProperty]
    bool editMode;

    [ObservableProperty]
    string buttonMode;

    [ObservableProperty]
    ImageSource buttonImageGetFromDb;

    protected readonly TestyDBContext _dbContext;
    protected readonly IMapper _mapper;

    public BaseCreatorViewModel(TestyDBContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    protected void UpdateButtonStates()
    {
        ButtonImageGetFromDb = (EditMode) ? "remove_item.png" : "get_from_db.png";
        ButtonMode = (EditMode) ? "Zatwierdź edycję" : "Dodaj";
    }

    protected bool SwitchEditMode()
    {
        EditMode = !EditMode;
        UpdateButtonStates();
        return EditMode;
    }

    protected bool AnswerExistsInQuestion(Pytanie question, Odpowiedz answer)
    {
        return question.Odpowiedzi.Any(odp => odp.IdOdpowiedzi == answer.IdOdpowiedzi
                                                 && odp.IdPytania == answer.IdPytania);
    }

    protected bool QuestionExistsInTest(Zestaw test, PytanieWZestawie question)
    {
        return test.PytaniaWZestawachNavigation.Any(odp => odp.IdZestawu == test.IdZestawu
                                                 && odp.IdPytania == question.IdPytania);
    }

    protected void RefreshIndexes(IEnumerable<IIndexable> items, string prefix)
    {
        int counter = 1;
        foreach(IIndexable item in items)
        {
            item.Idx = $"{prefix} {counter}";
            counter++;
        }
    }

    protected async Task ProceedConfirm(Func<Task> editFromDb, Func<Task> addToDb)
    {
        if (EditMode) await editFromDb();
        else await addToDb();
    }

    protected abstract void ResetFields();
    protected abstract void RegisterMessages(CreatorMessageType messageType);

    [RelayCommand]
    protected abstract void ClearAll();
    [RelayCommand]
    protected abstract Task Confirm();
}
