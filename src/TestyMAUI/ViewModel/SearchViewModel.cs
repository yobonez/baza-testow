using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Maui.DataGrid;
using System.Collections.ObjectModel;
using TestyLogic.Models;
using TestyMAUI.Messages;
using TestyMAUI.Services;
using TestyMAUI.UIModels;

namespace TestyMAUI.ViewModel;

public partial class SearchViewModel : ObservableObject
{
    #region props
    private readonly TestyDBContext _dbContext;
    private readonly IMapper _mapper;
    private readonly ViewModelLoader _viewModelLoader;

    internal bool _isFullQuestion;
    internal bool _isTestSearch;
    internal bool hasItemBeenSelected = false;

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
    #endregion

    public SearchViewModel(TestyDBContext dbContext, IMapper mapper, ViewModelLoader viewModelLoaders)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _viewModelLoader = viewModelLoaders;

        Pytania = new ObservableCollection<PytanieUI>();
        Zestawy = new ObservableCollection<ZestawUI>();
        fullPytania = new List<PytanieSearchEntryUI>();
        fullZestawy = new List<ZestawSearchEntryUI>();
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
        {
            (Pytania, fullPytania) = await _viewModelLoader.LoadAllQuestions(_isFullQuestion, subjectFilter);
            InitializeColumns(typeof(PytanieUI));
        }
        else
        { 
            fullZestawy = await _viewModelLoader.LoadAllTests(isFullTest:true, loadStats: false);
            Zestawy = new ObservableCollection<ZestawUI>(fullZestawy.Select(zest => zest.Zestaw));
            InitializeColumns(typeof(ZestawUI)); 
        }
    }

    [RelayCommand]
    async Task TapRefresh()
    {
        ResetSelection();
        await LoadAllItems(currentSubjectFilter);
    }

    internal void ResetSelection() => hasItemBeenSelected = false;

    async Task GoBack()
    {
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    async Task TapConfirm()
    {
        if (WybranePytanie == null && WybranyZestaw == null)
        {
            await Shell.Current.DisplayAlert("Błąd", "Nie wybrano żadnej pozycji.", "OK");
            return;
        }
        if (!_isTestSearch) {
            if (_isFullQuestion)
            {
                PytanieSearchEntryUI toSend = (from fullpyt in fullPytania
                                               where fullpyt.pytanie.Id == WybranePytanie.Id
                                               select fullpyt).Single();
                WeakReferenceMessenger.Default.Send<GetDetailedQuestionMessage>(new GetDetailedQuestionMessage(toSend));
            }

            else WeakReferenceMessenger.Default.Send<GetSimpleQuestionMessage>(new GetSimpleQuestionMessage(WybranePytanie));
        }
        else
        {
            ZestawSearchEntryUI toSend = (from fullzest in fullZestawy
                                           where fullzest.zestaw.Id == WybranyZestaw.Id
                                           select fullzest).Single();
            WeakReferenceMessenger.Default.Send<GetTestMessage>(new GetTestMessage(toSend));
        }

        hasItemBeenSelected = true;
        await GoBack();
    }
}
