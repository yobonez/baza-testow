using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using TestyLogic.Models;
using TestyMAUI.Messages;
using TestyMAUI.UIModels;

namespace TestyMAUI.ViewModel
{
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
        ZestawUI zestaw;

        [ObservableProperty]
        ObservableCollection<PytanieUI> pytania;

        [ObservableProperty]
        PrzedmiotUI wybranyPrzedmiot;

        [ObservableProperty]
        ObservableCollection<PrzedmiotUI> przedmioty;

        [RelayCommand]
        void AddQuestion()
        {
            Pytania.Add(new PytanieUI());
            RefreshQuestionIndexes();
        }

        [RelayCommand]
        async Task FetchQuestion()
        {
            RegisterQuestionMessage();
            await Shell.Current.GoToAsync($"{nameof(SearchPage)}?isFullQuestion=False&subjectFilter={WybranyPrzedmiot?.Nazwa}");
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
        private void RefreshQuestionIndexes()
        {
            int counter = 1;
            foreach (PytanieUI pyt in Pytania)
            {
                pyt.Idx = "Pytanie " + counter;
                counter++;
            }
        }

        // TODO: to-dry
        void RegisterQuestionMessage()
        {
            WeakReferenceMessenger.Default.Unregister<GetSimpleQuestionMessage>(this);
            WeakReferenceMessenger.Default.Register<GetSimpleQuestionMessage>(this, (r, m) =>
            {
                MainThread.BeginInvokeOnMainThread(() => {
                    PytanieSearchEntryUI received = m.Value;

                    Pytania.Add(new PytanieUI(received.pytanie.Id, received.pytanie.Tresc, received.pytanie.Punkty, received.pytanie.TypPytania));

                    WybranyPrzedmiot ??= Przedmioty.Single(p => p.Id == received.przedmiot.Id);

                    RefreshQuestionIndexes();
                });
            });
        }
    }
}
