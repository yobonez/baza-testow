using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.UI.Composition;
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
        ObservableCollection<PytanieSearchEntryUI> pytania;

        // send back to prev ui
        [ObservableProperty]
        PytanieSearchEntryUI wybranePytanie;
        public SearchViewModel(TestyDBContext dbContext)
        {
            _dbContext = dbContext;
            Pytania = new ObservableCollection<PytanieSearchEntryUI>();
        }

        public async Task LoadAllQuestions(bool forceRefresh = false)
        {
            if (!forceRefresh && Pytania.Count > 0)
                return;


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
                PytanieUI pytToSend = new PytanieUI(pyt.IdPytania, pyt.Tresc, pyt.Punkty, pyt.TypPytania);

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


                KategoriaUI katToSend = new KategoriaUI(kateg.IdKategorii, kateg.Nazwa);
                PrzedmiotUI przToSend = new PrzedmiotUI(przedm.IdPrzedmiotu, przedm.Nazwa);
                List<OdpowiedzUI> odpToSend = odpow.Select(odp => new OdpowiedzUI(odp.IdOdpowiedzi, odp.Tresc, odp.CzyPoprawna, odp.IdPytania))
                                                   .ToList();
                
                

                PytanieSearchEntryUI toSend = new PytanieSearchEntryUI(pytToSend, przToSend, katToSend, odpToSend);

                Pytania.Add(toSend);
            });
        }

        [RelayCommand]
        async Task TapRefresh()
        {
            await LoadAllQuestions(forceRefresh: true);
        }

        async Task GoBack()
        {
            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        async Task TapConfirm()
        {
            WeakReferenceMessenger.Default.Send<GetQuestionMessage>(new GetQuestionMessage(WybranePytanie));
            await GoBack();
        }
    }
}
