using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using TestyLogic.Models;
using TestyMAUI.Messages;
using TestyMAUI.UIModels;

namespace TestyMAUI.ViewModel;

public partial class TestsCreatorViewModel : ObservableObject
{
    private readonly TestyDBContext _dbContext;
    private readonly IMapper _mapper;

    public TestsCreatorViewModel(TestyDBContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;

        ResetFields();
    }

    List<Przedmiot> przedmiotyDto;

    [ObservableProperty]
    bool editMode;

    [ObservableProperty]
    string buttonMode;

    [ObservableProperty]
    ImageSource buttonImageGetFromDb;

    [ObservableProperty]
    ZestawUI zestaw;

    [ObservableProperty]
    ObservableCollection<PytanieUI> pytania;

    [ObservableProperty]
    PrzedmiotUI wybranyPrzedmiot;

    [ObservableProperty]
    ObservableCollection<PrzedmiotUI> przedmioty;

    PytanieUI questionToEdit;

    void AddQuestion()
    {
        Pytania.Add(new PytanieUI());
        RefreshQuestionIndexes();
    }

    [RelayCommand]
    async Task GetFromDb()
    {
        if(SwitchEditMode())
        {
            RegisterTestMessage();
            await Shell.Current.GoToAsync($"{nameof(SearchPage)}?isFullQuestion=False&subjectFilter={WybranyPrzedmiot?.Nazwa}&isTestSearch=True");
        }
        else
        {
            ResetFields();
        }
    }

    [RelayCommand]
    async Task FetchQuestion(PytanieUI questionParam)
    {
        questionToEdit = questionParam;

        RegisterQuestionMessage();
        await Shell.Current.GoToAsync($"{nameof(SearchPage)}?isFullQuestion=False&subjectFilter={WybranyPrzedmiot?.Nazwa}&isTestSearch=False");
    }

    [RelayCommand]
    void RemoveQuestion(PytanieUI question)
    {
        if (!Pytania.Contains(question) || Pytania.Count == 1)
            return;
        Pytania.Remove(question);
        RefreshQuestionIndexes();
    }
    [RelayCommand]
    void ClearAll() => ResetFields();


    void ResetFields()
    {
        Zestaw = new ZestawUI();
        Pytania = new() { new PytanieUI() };
        RefreshQuestionIndexes();
    }
    public async Task LoadSubjects()
    {
        przedmiotyDto = await _dbContext.Przedmioty.ToListAsync();
        Przedmioty = new ObservableCollection<PrzedmiotUI>(
            _mapper.Map<List<PrzedmiotUI>>(przedmiotyDto));
    }

    // TODO: to-dry
    void RefreshQuestionIndexes()
    {
        int counter = 1;
        foreach (PytanieUI pyt in Pytania)
        {
            pyt.Idx = "Pytanie " + counter;
            counter++;
        }
    }
    bool SwitchEditMode()
    {
        EditMode = !EditMode;
        UpdateButtonStates();

        return EditMode;
    }
    void UpdateButtonStates()
    {
        ButtonImageGetFromDb = (EditMode) ? "remove_item.png" : "get_from_db.png";
        ButtonMode = (EditMode) ? "Zatwierdź edycję" : "Dodaj";
    }

    // TODO: to-dry (RegisterMessages?)

    void RegisterTestMessage()
    {
        WeakReferenceMessenger.Default.Unregister<GetTestMessage>(this);
        WeakReferenceMessenger.Default.Register<GetTestMessage>(this, (r, m) =>
        {
            MainThread.BeginInvokeOnMainThread(() => {
                Zestaw = m.Value.zestaw;
                Pytania = new ObservableCollection<PytanieUI>(m.Value.pytania);
                WybranyPrzedmiot = m.Value.przedmiot;

                AddQuestion();

                RefreshQuestionIndexes();
            });
        });
    }
    void RegisterQuestionMessage()
    {
        WeakReferenceMessenger.Default.Unregister<GetSimpleQuestionMessage>(this);
        WeakReferenceMessenger.Default.Register<GetSimpleQuestionMessage>(this, (r, m) =>
        {
            MainThread.BeginInvokeOnMainThread(() => {
                PytanieSearchEntryUI received = m.Value;

                if(Pytania.Any(pyt => pyt.Id == received.pytanie.Id))
                {
                    AppShell.Current.DisplayAlert("Błąd", "To pytanie już istnieje w zestawie.", "OK");
                    return;
                }

                int toReplaceIndex = Pytania.IndexOf(Pytania.Single(pyt => pyt.Id == questionToEdit.Id));
                if (questionToEdit.Id != 0)
                {
                    Pytania.RemoveAt(toReplaceIndex);
                    Pytania.Insert(toReplaceIndex, new PytanieUI(received.pytanie.Id, received.pytanie.Tresc, received.pytanie.Punkty, received.pytanie.TypPytania));
                }
                else
                {
                    Pytania.Remove(Pytania.Last());
                    Pytania.Add(new PytanieUI(received.pytanie.Id, received.pytanie.Tresc, received.pytanie.Punkty, received.pytanie.TypPytania));
                    AddQuestion();
                }

                WybranyPrzedmiot ??= Przedmioty.Single(p => p.Id == received.przedmiot.Id);

                RefreshQuestionIndexes();
            });
        });
    }
}
