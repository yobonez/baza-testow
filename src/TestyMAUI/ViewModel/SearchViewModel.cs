using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using TestyLogic.Models;
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
        public SearchViewModel(TestyDBContext dbContext)
        {
            _dbContext = dbContext;
            Pytania = new ObservableCollection<PytanieUI>();
        }

        public async Task LoadAllQuestions(bool forceRefresh = false)
        {
            if (!forceRefresh && Pytania.Count > 0)
                return;


            var dbKategorie = await _dbContext.Kategorie.ToListAsync();
            var dbPytania = await _dbContext.Pytania.ToListAsync();
            dbPytania.ForEach(x => {
                PytanieUI toAdd = new PytanieUI(x.IdPytania, x.Tresc, x.Punkty, x.TypPytania);
                Pytania.Add(toAdd); 
            });

            
        }

        [RelayCommand]
        async Task TapRefresh()
        {
            await LoadAllQuestions(true);
        }
    }
}
