using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Maui.Animations;
using System.Collections.ObjectModel;
using TestyLogic.Models;
using TestyMAUI.Messages;
using TestyMAUI.UIModels;

namespace TestyMAUI.ViewModel
{
    public partial class SearchViewModel : ObservableObject
    {
        private readonly TestyDBContext _dbContext;

        [ObservableProperty]
        ObservableCollection<PytanieUI> pytania;

        // send back to prev ui
        [ObservableProperty]
        PytanieUI wybranePytanie;

        List<PytanieSearchEntryUI> fullPytania;

        public SearchViewModel(TestyDBContext dbContext)
        {
            _dbContext = dbContext;
            Pytania = new ObservableCollection<PytanieUI>();
            fullPytania = new List<PytanieSearchEntryUI>();
        }

        public async Task LoadAllQuestions()
        {
            Pytania.Clear();
            fullPytania.Clear();
            _dbContext.ChangeTracker.Clear();

            var dbPrzedmioty = await _dbContext.Przedmioty.ToListAsync();
            var dbKategorie = await _dbContext.Kategorie.ToListAsync();
            var dbOdpowiedzi = await _dbContext.Odpowiedzi.ToListAsync();
            var dbPytania = await _dbContext.Pytania
                .Include(el => el.PrzynaleznoscPytanNavigation)
                    .ThenInclude(subEl => subEl.IdPrzedmiotuNavigation)
                .Include(el => el.PrzynaleznoscPytanNavigation)
                    .ThenInclude(subEl => subEl.IdKategoriiNavigation)
                .Include(el => el.Odpowiedzi)
                .ToListAsync();



            dbPytania.ForEach(pyt =>
            {
                if (pyt.TypPytania == true) // not supporting open ones
                    return;

                PytanieUI pytToAdd = new PytanieUI(pyt.IdPytania, pyt.Tresc, pyt.Punkty, pyt.TypPytania);

                Kategoria kateg = (from kat in dbKategorie
                                   where kat.IdKategorii == (from przyn in pyt.PrzynaleznoscPytanNavigation
                                                             where przyn.IdPytania == pyt.IdPytania
                                                             select przyn.IdKategorii).Single()
                                   select kat).Single();

                Przedmiot przedm = (from prz in dbPrzedmioty
                                   where prz.IdPrzedmiotu == (from przyn in pyt.PrzynaleznoscPytanNavigation
                                                             where przyn.IdPytania == pyt.IdPytania
                                                             select przyn.IdPrzedmiotu).Single()
                                   select prz).Single();

                List<Odpowiedz> odpow = (from odp in dbOdpowiedzi
                                         where odp.IdPytania == pyt.IdPytania
                                         select odp).ToList();


                KategoriaUI katToAdd = new KategoriaUI(kateg.IdKategorii, kateg.Nazwa);
                PrzedmiotUI przToAdd = new PrzedmiotUI(przedm.IdPrzedmiotu, przedm.Nazwa);
                List<OdpowiedzUI> odpToAdd = odpow.Select(odp => new OdpowiedzUI(odp.IdOdpowiedzi, odp.Tresc, odp.CzyPoprawna, odp.IdPytania))
                                                   .ToList();


                fullPytania.Add(new PytanieSearchEntryUI(pytToAdd, przToAdd, katToAdd, odpToAdd));

                Pytania.Add(pytToAdd);
            });
        }

        [RelayCommand]
        async Task TapRefresh()
        {
            await LoadAllQuestions();
        }

        async Task GoBack()
        {
            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        async Task TapConfirm()
        {
            PytanieSearchEntryUI toSend = (from fullpyt in fullPytania
                                           where fullpyt.pytanie.Id == WybranePytanie.Id
                                           select fullpyt).Single();
            WeakReferenceMessenger.Default.Send<GetQuestionMessage>(new GetQuestionMessage(toSend));
            await GoBack();
        }
    }
}
