﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using TestyLogic.Models;
using TestyMAUI.UIModels;

namespace TestyMAUI.ViewModel
{
    public partial class QuestionsCreatorViewModel : ObservableObject
    {
        private readonly TestyDBContext _dbContext;

        public QuestionsCreatorViewModel(TestyDBContext dbContext) 
        {
            _dbContext = dbContext;

            ResetFields();
        }

        public void ResetFields()
        {
            PrzedmiotUI przedmiot = new(0, "");
            KategoriaUI kategoria = new(0, "");
            ObservableCollection<OdpowiedzUI> odpowiedzi = new() { new OdpowiedzUI(0, "", false, 0) };

            Pytanie = new PytanieUI(0, "", 1, false);
            WybranyPrzedmiot = przedmiot;
            WybranaKategoria = kategoria;
            Odpowiedzi = odpowiedzi;
        }

        public async Task LoadSubjectsNCategories()
        {
            // TODO: when loading a question, change selected item to the ones, that correspond to
            // question's subject and category
            List<Przedmiot> przedmiotyDto = await _dbContext.Przedmioty.ToListAsync();
            Przedmioty = new ObservableCollection<PrzedmiotUI> (przedmiotyDto.Select(
                el => new PrzedmiotUI() {
                    IdPrzedmiotu = el.IdPrzedmiotu, Nazwa = el.Nazwa 
                }
            ));

            List<Kategoria> kategorieDto = await _dbContext.Kategorie.ToListAsync();
            Kategorie = new ObservableCollection<KategoriaUI>(kategorieDto.Select(
                el => new KategoriaUI()
                {
                    IdKategorii = el.IdKategorii, Nazwa = el.Nazwa
                }
            ));
        }

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
        [RelayCommand]
        async Task LoadQuestion()
        {
            // Na razie ładowanie jednego pytania, trzeba zacząć od najprostszej rzeczy
            var pytanieDto = await _dbContext.Pytania
                .Include(el => el.PrzynaleznoscPytanNavigation)
                    .ThenInclude(subEl => subEl.IdPrzedmiotuNavigation)
                .Include(el => el.PrzynaleznoscPytanNavigation)
                    .ThenInclude(subEl => subEl.IdKategoriiNavigation)
                .Include(el => el.Odpowiedzi)
                .FirstOrDefaultAsync();

            Przedmiot przedmiotDto = pytanieDto.PrzynaleznoscPytanNavigation
                .Select(obj => obj.IdPrzedmiotuNavigation)
                .Take(1)
                .Single();
            Kategoria kategoriaDto = pytanieDto.PrzynaleznoscPytanNavigation
                .Select(obj => obj.IdKategoriiNavigation)
                .Take(1)
                .Single();

            Pytanie = new PytanieUI()
            {
                IdPytania = pytanieDto.IdPytania,
                Tresc = pytanieDto.Tresc,
                TypPytania = pytanieDto.TypPytania,
                Punkty = pytanieDto.Punkty
            };

            WybranyPrzedmiot = new PrzedmiotUI()
            {
                IdPrzedmiotu = przedmiotDto.IdPrzedmiotu,
                Nazwa = przedmiotDto.Nazwa
            };
            WybranaKategoria = new KategoriaUI()
            {
                IdKategorii = kategoriaDto.IdKategorii,
                Nazwa = kategoriaDto.Nazwa
            };
            Odpowiedzi = new ObservableCollection<OdpowiedzUI>(pytanieDto.Odpowiedzi.Select(el => new OdpowiedzUI()
            {
                IdPytania = el.IdPytania,
                Tresc = el.Tresc,
                CzyPoprawna = el.CzyPoprawna,
                IdOdpowiedzi = el.IdOdpowiedzi
            }));
        }
    }
}
