using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using TestyLogic.Models;
using TestyMAUI.Services;
using TestyMAUI.UIModels;

namespace TestyMAUI.ViewModel;

public partial class TestSelectorViewModel : ObservableObject
{
    private readonly TestyDBContext _dbContext;
    private readonly IMapper _mapper;
    private readonly ViewModelLoader _viewModelLoader;

    [ObservableProperty]
    ObservableCollection<ZestawSearchEntryUI> zestawy;

    public TestSelectorViewModel(TestyDBContext dbContext, IMapper mapper, ViewModelLoader viewModelLoader)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _viewModelLoader = viewModelLoader;
    }
}
