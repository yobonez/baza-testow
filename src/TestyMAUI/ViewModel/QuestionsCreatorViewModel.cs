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

public partial class QuestionsCreatorViewModel : BaseCreatorViewModel
{
    public QuestionsCreatorViewModel(TestyDBContext dbContext, IMapper mapper)
        : base(dbContext, mapper)
    {
        ResetFields();
    }

    #region props
    List<Przedmiot> przedmiotyDto;
    List<Kategoria> kategorieDto;
    //PytanieSearchEntryUI fullPytanie;

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

    protected override void ResetFields()
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
        RefreshIndexes(Odpowiedzi, "Odpowiedź");
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

    [RelayCommand]
    void AddAnswer()
    {
        Odpowiedzi.Add(new OdpowiedzUI(0, "", false, 0));
        RefreshCorrectnessIcons();
        RefreshIndexes(Odpowiedzi, "Odpowiedź");
    }
    [RelayCommand]
    void RemoveAnswer(OdpowiedzUI answer)
    {
        if (!Odpowiedzi.Contains(answer) || Odpowiedzi.Count == 1)
            return;
        Odpowiedzi.Remove(answer);
        RefreshIndexes(Odpowiedzi, "Odpowiedź");
    }
    protected override void ClearAll() => ResetFields();

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

    [RelayCommand]
    async Task GetFromDb()
    {
        if(SwitchEditMode())
        {
            RegisterMessages(CreatorMessageType.QuestionMessageFromQuestions);
            await Shell.Current.GoToAsync($"{nameof(SearchPage)}?isFullQuestion=True&isTestSearch=False");
        }
        else
        {
            ResetFields();
        }
    }

    protected override async Task Confirm() => await ProceedConfirm(EditQuestionFromDb, AddQuestionToDb);

    async Task EditQuestionFromDb()
    {
        Pytanie pytanieToEdit = SetupQuestion();
        Pytanie pytanieDb = _dbContext.Pytania
            .Include(el => el.PrzynaleznoscPytanNavigation)
                .ThenInclude(subEl => subEl.IdPrzedmiotuNavigation)
            .Include(el => el.PrzynaleznoscPytanNavigation)
                .ThenInclude(subEl => subEl.IdKategoriiNavigation)
            .Include(el => el.Odpowiedzi)
            .AsNoTracking()
            .Single(pyt => pyt.IdPytania == pytanieToEdit.IdPytania);

        foreach (Odpowiedz ans in pytanieDb.Odpowiedzi)
        {
            if (!AnswerExistsInQuestion(pytanieToEdit, ans))
                _dbContext.Entry(ans).State = EntityState.Deleted;
        }

        pytanieDb = pytanieToEdit;
        // ef core to enigma
        foreach (Odpowiedz ans in pytanieToEdit.Odpowiedzi)
        {
            if (ans.IdOdpowiedzi == 0)
                _dbContext.Entry(ans).State = EntityState.Added;
            else
                _dbContext.Entry(ans).State = EntityState.Modified;
        }

        _dbContext.Pytania.Update(pytanieDb);

        _dbContext.SaveChanges();

        _dbContext.Entry(pytanieDb).State = EntityState.Detached; // edit the same thing more than once

        ResetFields();
        SwitchEditMode();
        await AppShell.Current.DisplayAlert("Edycja pytania", "Pomyślnie edytowano pytanie.", "OK");
    }

    async Task AddQuestionToDb()
    {
        Pytanie pytanieToAdd = SetupQuestion();
        await _dbContext.Pytania.AddAsync(pytanieToAdd);
        await _dbContext.SaveChangesAsync();
        ResetFields();

        _dbContext.Entry(pytanieToAdd).State = EntityState.Detached;

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

    protected override void RegisterMessages(CreatorMessageType messageType)
    {
        if (messageType == CreatorMessageType.QuestionMessageFromQuestions)
            RegisterQuestionMessage();
    }

    void RegisterQuestionMessage()
    {
        WeakReferenceMessenger.Default.Unregister<GetDetailedQuestionMessage>(this);
        WeakReferenceMessenger.Default.Register<GetDetailedQuestionMessage>(this, (r, m) =>
        {
            MainThread.BeginInvokeOnMainThread(() => {
                PytanieSearchEntryUI received = m.Value;

                Pytanie = new PytanieUI(received.pytanie.Id, received.pytanie.Tresc, received.pytanie.Punkty, received.pytanie.TypPytania);

                WybranyPrzedmiot = Przedmioty.Single(p => p.Id == received.przedmiot.Id);

                WybranaKategoria = Kategorie.Single(k => k.Id == received.kategoria.Id);

                Odpowiedzi = new ObservableCollection<OdpowiedzUI>(received.odpowiedzi);

                RefreshCorrectnessIcons();
                RefreshIndexes(Odpowiedzi, "Odpowiedź");
                if (Pytanie.Id == 0)
                    SwitchEditMode();
            }); 
        });
    }
}
