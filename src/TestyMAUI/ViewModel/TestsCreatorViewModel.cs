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

public partial class TestsCreatorViewModel : BaseCreatorViewModel
{

    public TestsCreatorViewModel(TestyDBContext dbContext, IMapper mapper) 
        : base(dbContext, mapper)
    {
        ResetFields();
    }

    #region props
    List<Przedmiot> przedmiotyDto;

    [ObservableProperty]
    ZestawUI zestaw;

    [ObservableProperty]
    ObservableCollection<PytanieUI> pytania;

    [ObservableProperty]
    PrzedmiotUI wybranyPrzedmiot;

    [ObservableProperty]
    ObservableCollection<PrzedmiotUI> przedmioty;

    PytanieUI questionToEdit;
    #endregion props

    void AddQuestion()
    {
        Pytania.Add(new PytanieUI());
        RefreshIndexes(Pytania, "Pytanie");
    }

    [RelayCommand]
    async Task GetFromDb()
    {
        if(SwitchEditMode())
        {
            RegisterMessages(CreatorMessageType.TestMessage);
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

        RegisterMessages(CreatorMessageType.QuestionMessageFromTests);
        await Shell.Current.GoToAsync($"{nameof(SearchPage)}?isFullQuestion=False&subjectFilter={WybranyPrzedmiot?.Nazwa}&isTestSearch=False");
    }

    [RelayCommand]
    void RemoveQuestion(PytanieUI question)
    {
        if (!Pytania.Contains(question) || Pytania.Count == 1)
            return;
        Pytania.Remove(question);
        RefreshIndexes(Pytania, "Pytanie");
        
        if (Pytania.Count == 1)
            WybranyPrzedmiot = null;
    }
    
    protected override void ClearAll() => ResetFields();

    protected override void ResetFields()
    {
        Zestaw = new ZestawUI();
        Pytania = new() { new PytanieUI() };
        RefreshIndexes(Pytania, "Pytanie");
        UpdateButtonStates();
        WybranyPrzedmiot = null;
    }
    public async Task LoadSubjects()
    {
        przedmiotyDto = await _dbContext.Przedmioty.ToListAsync();
        Przedmioty = new ObservableCollection<PrzedmiotUI>(
            _mapper.Map<List<PrzedmiotUI>>(przedmiotyDto));
    }

    protected override async Task Confirm() => await ProceedConfirm(EditTestFromDb, AddTestToDb);

    async Task AddTestToDb()
    {
        Zestaw zestawToAdd = SetupZestaw();

        await _dbContext.Zestawy.AddAsync(zestawToAdd);
        await _dbContext.SaveChangesAsync();
        ResetFields();

        _dbContext.Entry(zestawToAdd).State = EntityState.Detached;

        await AppShell.Current.DisplayAlert("Sukces", "Zestaw został dodany.", "OK");
    }

    async Task EditTestFromDb()
    {
        Zestaw zestawToEdit = SetupZestaw();
        Zestaw zestawDb = _dbContext.Zestawy
            .Include(z => z.PytaniaWZestawachNavigation)
            .AsNoTracking()
            .Single(z => z.IdZestawu == zestawToEdit.IdZestawu);

        foreach(PytanieWZestawie pytanieWZestawie in zestawDb.PytaniaWZestawachNavigation)
        {
            if(!QuestionExistsInTest(zestawToEdit, pytanieWZestawie))
                _dbContext.Entry(pytanieWZestawie).State = EntityState.Deleted;

        }
        
        foreach(PytanieWZestawie pytanieWZestawie in zestawToEdit.PytaniaWZestawachNavigation)
        {
            if (QuestionExistsInTest(zestawDb, pytanieWZestawie))
                _dbContext.Entry(pytanieWZestawie).State = EntityState.Modified;
            else
                _dbContext.Entry(pytanieWZestawie).State = EntityState.Added;
        }

        zestawDb = zestawToEdit;

        _dbContext.Zestawy.Update(zestawDb);
        _dbContext.SaveChanges();
        _dbContext.Entry(zestawDb).State = EntityState.Detached; // edit the same thing more than once

        ResetFields();
        SwitchEditMode();
        await AppShell.Current.DisplayAlert("Edycja zestawu", "Pomyślnie edytowano zestaw.", "OK");
    }

    Zestaw SetupZestaw()
    {
        Zestaw zestaw = _mapper.Map<Zestaw>(Zestaw);
        zestaw.IdPrzedmiotu = WybranyPrzedmiot.Id;
        zestaw.PytaniaWZestawachNavigation = _mapper.Map<List<PytanieWZestawie>>(
            Pytania
                .Where(pyt => pyt.Id != 0)
                .Select(pyt =>  
                    new PytanieWZestawie
                    {
                        IdPytania = pyt.Id,
                        IdZestawu = Zestaw.Id,
                        IdPrzedmiotu = WybranyPrzedmiot.Id
                    }).ToList()
        );

        return zestaw;
    }

    protected override void RegisterMessages(CreatorMessageType messageType)
    {
        if (messageType == CreatorMessageType.TestMessage)
            RegisterTestMessage();
        else if (messageType == CreatorMessageType.QuestionMessageFromTests)
            RegisterQuestionMessage();
    }

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

                RefreshIndexes(Pytania, "Pytanie");
            });
        });
    }
    void RegisterQuestionMessage()
    {
        WeakReferenceMessenger.Default.Unregister<GetSimpleQuestionMessage>(this);
        WeakReferenceMessenger.Default.Register<GetSimpleQuestionMessage>(this, (r, m) =>
        {
            MainThread.BeginInvokeOnMainThread(() => {
                PytanieUI received = m.Value;

                if(Pytania.Any(pyt => pyt.Id == received.Id))
                {
                    AppShell.Current.DisplayAlert("Błąd", "To pytanie już istnieje w zestawie.", "OK");
                    return;
                }

                int toReplaceIndex = Pytania.IndexOf(Pytania.Single(pyt => pyt.Id == questionToEdit.Id));
                if (questionToEdit.Id != 0)
                {
                    Pytania.RemoveAt(toReplaceIndex);
                    Pytania.Insert(toReplaceIndex, new PytanieUI(received.Id, received.Tresc, received.Punkty, received.TypPytania));
                }
                else
                {
                    Pytania.Remove(Pytania.Last());
                    Pytania.Add(new PytanieUI(received.Id, received.Tresc, received.Punkty, received.TypPytania));
                    AddQuestion();
                }

                WybranyPrzedmiot ??= _mapper.Map<Przedmiot, PrzedmiotUI>
                                            ((from przyn in _dbContext.PrzynaleznoscPytan
                                              where przyn.IdPytania == received.Id
                                              select przyn.IdPrzedmiotuNavigation).Single());

                RefreshIndexes(Pytania, "Pytanie");
            });
        });
    }
}
