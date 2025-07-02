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

public partial class QuestionsCreatorViewModel : ObservableObject
{
    private readonly TestyDBContext _dbContext;
    private readonly IMapper _mapper;

    public QuestionsCreatorViewModel(TestyDBContext dbContext, IMapper mapper) 
    {
        _dbContext = dbContext;
        _mapper = mapper;

        UpdateButtonStates();
        ResetFields();
    }

    #region props
    List<Przedmiot> przedmiotyDto;
    List<Kategoria> kategorieDto;
    //PytanieSearchEntryUI fullPytanie;

    [ObservableProperty]
    bool editMode;

    [ObservableProperty]
    string buttonMode;

    [ObservableProperty]
    ImageSource buttonImageGetFromDb;

    [ObservableProperty]
    PytanieUI pytanie;

    [ObservableProperty]
    ObservableCollection<OdpowiedzUI> odpowiedzi;

    [ObservableProperty]
    PrzedmiotUI wybranyPrzedmiot;

    [ObservableProperty]
    KategoriaUI wybranaKategoria;

    [ObservableProperty]
    ObservableCollection<PrzedmiotUI> przedmioty;

    [ObservableProperty]
    ObservableCollection<KategoriaUI> kategorie;
    #endregion props

    void UpdateButtonStates()
    {
        ButtonImageGetFromDb = (EditMode) ? "remove_item.png" : "get_from_db.png";
        ButtonMode = (EditMode) ? "Zatwierdź edycję" : "Dodaj";
    }
    void ResetFields()
    {
        PrzedmiotUI przedmiot = new(0, "");
        KategoriaUI kategoria = new(0, "");
        ObservableCollection<OdpowiedzUI> odpowiedzi = new() { new OdpowiedzUI(0, "", false, 0) };

        Pytanie = new PytanieUI(0, "", 5, false);
        WybranyPrzedmiot = przedmiot;
        WybranaKategoria = kategoria;
        Odpowiedzi = odpowiedzi;

        RefreshCorrectnessIcons();
        UpdateButtonStates();
        RefreshAnswersIndexes();
    }

    public async Task LoadSubjectsNCategories()
    {
        przedmiotyDto = await _dbContext.Przedmioty.ToListAsync();
        Przedmioty = new ObservableCollection<PrzedmiotUI>(
            _mapper.Map<List<PrzedmiotUI>>(przedmiotyDto));

        kategorieDto = await _dbContext.Kategorie.ToListAsync();
        Kategorie = new ObservableCollection<KategoriaUI>(
            _mapper.Map<List<KategoriaUI>>(kategorieDto));
    }

    private void RefreshAnswersIndexes()
    {
        int counter = 1;
        foreach(OdpowiedzUI odp in Odpowiedzi) {
            odp.Idx = "Odpowiedź " + counter;
            counter++;
        }
    }

    [RelayCommand]
    void AddAnswer()
    {
        Odpowiedzi.Add(new OdpowiedzUI(0, "", false, 0));
        RefreshCorrectnessIcons();
        RefreshAnswersIndexes();
    }
    [RelayCommand]
    void RemoveAnswer(OdpowiedzUI answer)
    {
        if (!Odpowiedzi.Contains(answer) || Odpowiedzi.Count == 1)
            return;
        Odpowiedzi.Remove(answer);
        RefreshAnswersIndexes();
    }
    [RelayCommand]
    void ClearAll()
    {
        ResetFields();
    }

    [RelayCommand]
    void SwitchCorrectness(OdpowiedzUI answer)
    {
        if (!Odpowiedzi.Contains(answer))
            return;
        answer.CorrectnessIcon = (answer.CzyPoprawna) ? "is_incorrect.png" : "is_correct.png";
        answer.CzyPoprawna = !answer.CzyPoprawna;
    }
    void RefreshCorrectnessIcons()
    {
        foreach (var answer in Odpowiedzi)
        {
            answer.CorrectnessIcon = (answer.CzyPoprawna) ? "is_correct.png" : "is_incorrect.png";
        }
    }

    bool SwitchEditMode()
    {
        EditMode = !EditMode;
        UpdateButtonStates();

        return EditMode;
    }

    [RelayCommand]
    async Task TapGetFromDb()
    {
        if(SwitchEditMode())
        {
            RegisterQuestionMessage();
            await Shell.Current.GoToAsync(nameof(SearchPage));
        }
        else
        {
            ResetFields();
        }
    }

    [RelayCommand]
    async Task Confirm()
    {   
        if (EditMode) await EditQuestionFromDb();
        else await AddQuestionToDb();
    }

    async Task EditQuestionFromDb()
    {
        Pytanie pytanieToEdit = SetupQuestion();

        foreach (Odpowiedz existingOdpowiedz in _dbContext.Odpowiedzi)
        {
            _dbContext.Entry(existingOdpowiedz).State = EntityState.Detached;
            if (!pytanieToEdit.Odpowiedzi.Any(odp => odp.IdOdpowiedzi == existingOdpowiedz.IdOdpowiedzi)) 
                _dbContext.Odpowiedzi.Remove(existingOdpowiedz);
        }

        _dbContext.Pytania.Update(pytanieToEdit);
        _dbContext.SaveChanges();
        _dbContext.Entry(pytanieToEdit).State = EntityState.Detached; // edit the same thing more than once

        ResetFields();
        SwitchEditMode();
        await AppShell.Current.DisplayAlert("Edycja pytania", "Pomyślnie edytowano pytanie.", "OK");
    }

    async Task AddQuestionToDb()
    {
        Pytanie pytanieToAdd = SetupQuestion();
        await _dbContext.Pytania.AddAsync(pytanieToAdd);
        ResetFields();
        await _dbContext.SaveChangesAsync();

        _dbContext.Entry(pytanieToAdd).State = EntityState.Detached; // edit the same thing more than once

        await AppShell.Current.DisplayAlert("Dodanie pytania", "Pytanie zostało dodane.", "OK");
    }

    Pytanie SetupQuestion()
    {
        Pytanie pytanie = _mapper.Map<Pytanie>(Pytanie);
        pytanie.Odpowiedzi = _mapper.Map<List<Odpowiedz>>(Odpowiedzi.ToList());

        if (!EditMode)
        {
            pytanie.PrzynaleznoscPytanNavigation = new List<PrzynaleznoscPytania>
            {
                new PrzynaleznoscPytania
                {
                    IdKategorii = WybranaKategoria.Id,
                    IdPrzedmiotu = WybranyPrzedmiot.Id
                }   
            };
        }
        return pytanie;
    }

    void RegisterQuestionMessage()
    {
        WeakReferenceMessenger.Default.Unregister<GetQuestionMessage>(this);
        WeakReferenceMessenger.Default.Register<GetQuestionMessage>(this, (r, m) =>
        {
            MainThread.BeginInvokeOnMainThread(() => {
                PytanieSearchEntryUI received = m.Value;

                Pytanie = new PytanieUI(received.pytanie.Id, received.pytanie.Tresc, received.pytanie.Punkty, received.pytanie.TypPytania);

                // TODO: maybe its enough to select from existing ones, eh?
                WybranyPrzedmiot = new PrzedmiotUI(received.przedmiot.Id, received.przedmiot.Nazwa);

                WybranaKategoria = new KategoriaUI(received.kategoria.Id, received.kategoria.Nazwa);

                Odpowiedzi = new ObservableCollection<OdpowiedzUI>(received.odpowiedzi);

                RefreshCorrectnessIcons();
                RefreshAnswersIndexes();
                if (Pytanie.Id == 0)
                    SwitchEditMode();
            }); 
        });
    }
}
