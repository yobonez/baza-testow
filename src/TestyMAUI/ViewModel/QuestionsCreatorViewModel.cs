using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Collections.ObjectModel;
using TestyLogic.Models;
using TestyMAUI.Messages;
using TestyMAUI.UIModels;

namespace TestyMAUI.ViewModel
{
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

            //fullPytanie = new PytanieSearchEntryUI(Pytanie, WybranyPrzedmiot, WybranaKategoria, Odpowiedzi.ToList());
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
        }
        [RelayCommand]
        void RemoveAnswer(OdpowiedzUI answer)
        {
            if (!Odpowiedzi.Contains(answer))
                return;
            Odpowiedzi.Remove(answer);
        }
        [RelayCommand]
        void ClearAll()
        {
            ResetFields();
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
            // only if there's data. If there is no data, then immediately change to db icon, so user
            // can interact after cancelling

            // if returned data null, then immediately db icon, edit mode off

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
            if (EditMode) EditQuestionFromDb();
            else AddQuestionToDb();

            SwitchEditMode();
        }

        private void EditQuestionFromDb()
        {
            throw new NotImplementedException();
        }

        async void AddQuestionToDb()
        {
            Pytanie pytanieToAdd = SetupQuestion();
            foreach (var odp in pytanieToAdd.Odpowiedzi)
                _dbContext.Entry(odp).State = EntityState.Added;
            foreach (var przyn in pytanieToAdd.PrzynaleznoscPytanNavigation)
                _dbContext.Entry(przyn).State = EntityState.Added;
            _dbContext.Entry(pytanieToAdd).State = EntityState.Added;
            await _dbContext.SaveChangesAsync();
        }

        Pytanie SetupQuestion()
        {
            Pytanie pytanie = _mapper.Map<Pytanie>(Pytanie);
            pytanie.Odpowiedzi = _mapper.Map<List<Odpowiedz>>(Odpowiedzi.ToList());
            pytanie.PrzynaleznoscPytanNavigation = new List<PrzynaleznoscPytania>
            {
                new PrzynaleznoscPytania
                {
                    IdKategorii = WybranaKategoria.Id,
                    IdPrzedmiotu = WybranyPrzedmiot.Id
                }
            };
            

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

                    WybranyPrzedmiot = new PrzedmiotUI(received.przedmiot.Id, received.przedmiot.Nazwa);

                    WybranaKategoria = new KategoriaUI(received.kategoria.Id, received.kategoria.Nazwa);

                    Odpowiedzi = new ObservableCollection<OdpowiedzUI>(received.odpowiedzi);

                    if (Pytanie.Id == 0)
                        SwitchEditMode();
                }); 
            });
        }
    }
}
