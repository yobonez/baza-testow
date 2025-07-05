using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Maui.DataGrid;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using TestyLogic.Models;
using TestyMAUI.Messages;
using TestyMAUI.UIModels;

namespace TestyMAUI.ViewModel;

public partial class SearchViewModel : ObservableObject
{
    private readonly TestyDBContext _dbContext;
    private readonly IMapper _mapper;

    internal bool _isFullQuestion;
    internal bool _isTestSearch;

    string currentSubjectFilter;

    [ObservableProperty]
    internal ObservableCollection<DataGridColumn> columns;

    [ObservableProperty]
    internal ObservableCollection<PytanieUI> pytania;

    [ObservableProperty]
    internal ObservableCollection<ZestawUI> zestawy;

    [ObservableProperty]
    internal PytanieUI wybranePytanie;

    [ObservableProperty]
    internal ZestawUI wybranyZestaw;

    List<PytanieSearchEntryUI> fullPytania;
    List<ZestawSearchEntryUI> fullZestawy;

    public SearchViewModel(TestyDBContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        Pytania = new ObservableCollection<PytanieUI>();
        Zestawy = new ObservableCollection<ZestawUI>();
        fullPytania = new List<PytanieSearchEntryUI>();
        fullZestawy = new List<ZestawSearchEntryUI>();
    }


    async Task LoadAllQuestions(string? subjectFilter = null)
    {
        Pytania.Clear();
        fullPytania.Clear();

        _dbContext.ChangeTracker.Clear(); // avoid weird stuff happening with EF Core tracking affecting the UI

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

            subjectFilter = (subjectFilter == "") ? null : subjectFilter;
            if ((subjectFilter != null)
                && !pyt.PrzynaleznoscPytanNavigation
                       .Any(prz => prz.IdPrzedmiotuNavigation.Nazwa == subjectFilter))
                return;

            PytanieUI pytToAdd = new PytanieUI(pyt.IdPytania, pyt.Tresc, pyt.Punkty, pyt.TypPytania);

            PrzedmiotUI przToAdd = _mapper.Map<PrzedmiotUI>(
                (from prz in dbPrzedmioty
                                where prz.IdPrzedmiotu == (from przyn in pyt.PrzynaleznoscPytanNavigation
                                                           where przyn.IdPytania == pyt.IdPytania
                                                           select przyn.IdPrzedmiotu).Single()
                                select prz).Single()
            );


            if (!_isFullQuestion)
                fullPytania.Add(new PytanieSearchEntryUI(pytToAdd, przToAdd));
            else
            {
                KategoriaUI katToAdd = _mapper.Map<KategoriaUI>(
                                   (from kat in dbKategorie
                                   where kat.IdKategorii == (from przyn in pyt.PrzynaleznoscPytanNavigation
                                                             where przyn.IdPytania == pyt.IdPytania
                                                             select przyn.IdKategorii).Single()
                                   select kat).Single()
                );

                List<OdpowiedzUI> odpToAdd = _mapper.Map<List<OdpowiedzUI>>((from odp in dbOdpowiedzi
                                         where odp.IdPytania == pyt.IdPytania
                                         select odp).ToList());


                fullPytania.Add(new PytanieSearchEntryUI(pytToAdd, przToAdd, katToAdd, odpToAdd));
            }

            Pytania.Add(pytToAdd);
        });
    }
    async Task LoadAllTests()
    {
        Zestawy.Clear();
        fullZestawy.Clear();

        _dbContext.ChangeTracker.Clear();

        var dbPrzedmioty = await _dbContext.Przedmioty.ToListAsync();
        var dbKategorie = await _dbContext.Kategorie.ToListAsync();
        var dbZestawy = await _dbContext.Zestawy
            .Include(el => el.PytaniaWZestawachNavigation)
                .ThenInclude(subEl => subEl.IdPytaniaNavigation)
            .Include(el => el.PytaniaWZestawachNavigation)
                .ThenInclude(subEl => subEl.IdPrzedmiotuNavigation)
            .ToListAsync();

        dbZestawy.ForEach(zestaw =>
        {
            ZestawUI zestToAdd = new ZestawUI(zestaw.IdZestawu, zestaw.Nazwa, zestaw.DataUtworzenia, zestaw.IdPrzedmiotu);

            PrzedmiotUI przToAdd = _mapper.Map<PrzedmiotUI>((from prz in dbPrzedmioty
                                  where prz.IdPrzedmiotu == zestaw.IdPrzedmiotu
                                  select prz)
                               .Single());
            List<PytanieUI> pytToAdd = _mapper.Map<List<PytanieUI>>(
                (from zest in dbZestawy
                 where zest.IdZestawu == zestaw.IdZestawu
                 select zest.PytaniaWZestawachNavigation.Select(pytwz => pytwz.IdPytaniaNavigation)).Single()
            );

            fullZestawy.Add(new ZestawSearchEntryUI(zestToAdd, przToAdd, pytToAdd));
            Zestawy.Add(zestToAdd);
        });

    }

    void InitializeColumns(Type searchType)
    {
        Columns = new ObservableCollection<DataGridColumn>();
        foreach (var prop in searchType.GetProperties())
        {
            if (searchType == typeof(PytanieUI) || searchType == typeof(ZestawUI))
            {
                if (prop.Name == nameof(PytanieUI.Id) || 
                    prop.Name == nameof(ZestawUI.Id) ||
                    prop.Name == nameof(PytanieUI.Idx))
                    continue; 
            }
            Columns.Add(new DataGridColumn()
                { Title = prop.Name, PropertyName = prop.Name }
            );
        }
    }
    public async Task LoadAllItems(string? subjectFilter = null)
    {
        currentSubjectFilter = subjectFilter ?? string.Empty;

        if (!_isTestSearch)
        { await LoadAllQuestions(subjectFilter); InitializeColumns(typeof(PytanieUI)); }
        else
        { await LoadAllTests(); InitializeColumns(typeof(ZestawUI)); }
    }

    [RelayCommand]
    async Task TapRefresh()
    {
        await LoadAllItems(currentSubjectFilter);
    }

    async Task GoBack()
    {
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    async Task TapConfirm()
    {
        if (!_isTestSearch) { 
            PytanieSearchEntryUI toSend = (from fullpyt in fullPytania
                                           where fullpyt.pytanie.Id == WybranePytanie.Id
                                           select fullpyt).Single();

            if (!_isFullQuestion) WeakReferenceMessenger.Default.Send<GetSimpleQuestionMessage>(new GetSimpleQuestionMessage(toSend));
            else WeakReferenceMessenger.Default.Send<GetDetailedQuestionMessage>(new GetDetailedQuestionMessage(toSend));
        }
        else
        {
            ZestawSearchEntryUI toSend = (from fullzest in fullZestawy
                                           where fullzest.zestaw.Id == WybranyZestaw.Id
                                           select fullzest).Single();
            WeakReferenceMessenger.Default.Send<GetTestMessage>(new GetTestMessage(toSend));
        }

        await GoBack();
    }
}
