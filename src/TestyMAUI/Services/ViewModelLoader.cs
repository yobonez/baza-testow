using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using TestyLogic.Models;
using TestyMAUI.UIModels;
using TestyMAUI.ViewModel;

namespace TestyMAUI.Services;


public class ViewModelLoader
{
    private readonly TestyDBContext _dbContext;
    private readonly IMapper _mapper;

    public ViewModelLoader(TestyDBContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    private async  
        Task<(List<Przedmiot>, List<Zestaw>)> 
        LoadSubjectsAndTests()
    {
        _dbContext.ChangeTracker.Clear();

        var dbPrzedmioty = await _dbContext.Przedmioty.ToListAsync();
        var dbZestawy = await  _dbContext.Zestawy
            .Include(el => el.PytaniaWZestawachNavigation)
                .ThenInclude(subEl => subEl.IdPytaniaNavigation)
            .Include(el => el.PytaniaWZestawachNavigation)
                .ThenInclude(subEl => subEl.IdPrzedmiotuNavigation)
            .ToListAsync();

        return (dbPrzedmioty, dbZestawy);
    }

    private async 
        Task<(List<Przedmiot>, List<Pytanie>, List<Kategoria>, List<Odpowiedz>)> 
        LoadQuestionsCategoriesNAnswers()
    {
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
        
        return (dbPrzedmioty, dbPytania, dbKategorie, dbOdpowiedzi);
    }

    private 
        (ZestawUI, PrzedmiotUI) 
        GetNextTestElement(Zestaw zestaw, List<Przedmiot> dbPrzedmioty)
    {
        ZestawUI zestToAdd = new ZestawUI(zestaw.IdZestawu, zestaw.Nazwa, zestaw.DataUtworzenia, zestaw.IdPrzedmiotu);

        PrzedmiotUI przToAdd = _mapper.Map<PrzedmiotUI>((from prz in dbPrzedmioty
                                                        where prz.IdPrzedmiotu == zestaw.IdPrzedmiotu
                                                        select prz)
                            .Single());

        return (zestToAdd, przToAdd);
    }

    private 
        (PytanieUI, PrzedmiotUI) 
        GetNextSimpleQuestionElement(Pytanie pyt, List<Przedmiot> dbPrzedmioty)
    {
        PytanieUI pytToAdd = new PytanieUI(pyt.IdPytania, pyt.Tresc, pyt.Punkty, pyt.TypPytania);

        PrzedmiotUI przToAdd = _mapper.Map<PrzedmiotUI>(
            (from prz in dbPrzedmioty
             where prz.IdPrzedmiotu == (from przyn in pyt.PrzynaleznoscPytanNavigation
                                        where przyn.IdPytania == pyt.IdPytania
                                        select przyn.IdPrzedmiotu).Single()
             select prz).Single()
        );

        return (pytToAdd, przToAdd);
    }

    private 
        (KategoriaUI, List<OdpowiedzUI>) 
        GetNextDetailedQuestionElement (Pytanie pyt, List<Kategoria> dbKategorie, List<Odpowiedz> dbOdpowiedzi)
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
        return (katToAdd, odpToAdd);
    }

    public async 
        Task<List<ZestawSearchEntryUI>> 
        LoadAllTests(bool isFullTest, bool loadStats)
    {
        List<ZestawSearchEntryUI> fullZestawy = new();

        var (dbPrzedmioty, dbZestawy) = await LoadSubjectsAndTests();

        dbZestawy.ForEach(zestaw =>
        {
            var (zestToAdd, przToAdd) = GetNextTestElement(zestaw, dbPrzedmioty);
            int? iloscPkt = (loadStats) ? _dbContext.Zestawy.Where(el => el.IdZestawu == zestaw.IdZestawu)
                                                            .Select(el => _dbContext.IloscPunktowZestaw(el.IdZestawu)).Single() : null,
                 iloscPyt = (loadStats) ? _dbContext.Zestawy.Where(el => el.IdZestawu == zestaw.IdZestawu)
                                                            .Select(el => _dbContext.IloscPytanZestaw(el.IdZestawu)).Single() : null;

            List<PytanieUI>? pytToAdd = (isFullTest) 
            ? 
            _mapper.Map<List<PytanieUI>>
            (
                (from zest in dbZestawy
                 where zest.IdZestawu == zestaw.IdZestawu
                 select zest.PytaniaWZestawachNavigation.Select(pytwz => pytwz.IdPytaniaNavigation)).Single()
            ) : null;

            fullZestawy.Add(new ZestawSearchEntryUI(zestToAdd, przToAdd, pytToAdd, iloscPyt, iloscPkt));
        });

        return fullZestawy;
    }

    public async 
        Task<(ObservableCollection<PytanieUI>, List<PytanieSearchEntryUI>?)> 
        LoadAllQuestions(bool isFullQuestion, string? subjectFilter = null)
    {
        ObservableCollection<PytanieUI> pytania = new();
        List<PytanieSearchEntryUI>? fullPytania = 
            isFullQuestion ? new() : null;

        var (dbPrzedmioty, dbPytania, dbKategorie, dbOdpowiedzi) = await LoadQuestionsCategoriesNAnswers();

        dbPytania.ForEach(pyt =>
        {
            if (pyt.TypPytania == true) // not supporting open ones
                return;

            subjectFilter = subjectFilter == "" ? null : subjectFilter;
            if (subjectFilter != null
                && !pyt.PrzynaleznoscPytanNavigation
                       .Any(prz => prz.IdPrzedmiotuNavigation.Nazwa == subjectFilter))
                return;

            var (pytToAdd, przToAdd) = GetNextSimpleQuestionElement(pyt, dbPrzedmioty);


            if (!isFullQuestion && fullPytania is not null)
                fullPytania.Add(new PytanieSearchEntryUI(pytToAdd, przToAdd));
            else if (fullPytania is not null)
            {
                var (katToAdd, odpToAdd) = GetNextDetailedQuestionElement(pyt, dbKategorie, dbOdpowiedzi);

                fullPytania.Add(new PytanieSearchEntryUI(pytToAdd, przToAdd, katToAdd, odpToAdd));
            }

            pytania.Add(pytToAdd);
        });

        return (pytania, fullPytania);
    }

    public async Task<PytanieSearchEntryUI?> LoadFullQuestionFromTest(int idZestawu, int idPytania)
    {
        PytanieSearchEntryUI? question = await _dbContext.Pytania
            .Include(el => el.PrzynaleznoscPytanNavigation)
                .ThenInclude(subEl => subEl.IdPrzedmiotuNavigation)
            .Include(el => el.PrzynaleznoscPytanNavigation)
                .ThenInclude(subEl => subEl.IdKategoriiNavigation)
            .Include(el => el.Odpowiedzi)
            .Where(el => el.PytaniaWZestawachNavigation.Any(pytwz => pytwz.IdZestawu == idZestawu && pytwz.IdPytania == idPytania))
            .Select(pyt => new PytanieSearchEntryUI
            (
                new PytanieUI(pyt.IdPytania, pyt.Tresc, pyt.Punkty, pyt.TypPytania),
                _mapper.Map<PrzedmiotUI>(pyt.PrzynaleznoscPytanNavigation.First().IdPrzedmiotuNavigation),
                _mapper.Map<KategoriaUI>(pyt.PrzynaleznoscPytanNavigation.First().IdKategoriiNavigation),
                _mapper.Map<List<OdpowiedzUI>>(pyt.Odpowiedzi.ToList())
            ))
            .OrderBy(el => Guid.NewGuid())
            .SingleOrDefaultAsync();
            
            question.Odpowiedzi.ForEach(odp => 
            {
                odp.CzyPoprawna = false;
            });
        return question;
    }
}
