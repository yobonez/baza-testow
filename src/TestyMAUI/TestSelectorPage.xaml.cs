using System.Collections.ObjectModel;
using TestyMAUI.Services;
using TestyMAUI.UIModels;
using TestyMAUI.ViewModel;

namespace TestyMAUI;

public partial class TestSelectorPage : ContentPage
{
    private TestSelectorViewModel viewModel;
    private ViewModelLoader _viewModelLoader;

	public TestSelectorPage(TestSelectorViewModel vm, ViewModelLoader viewModelLoader)
	{
		InitializeComponent();
        viewModel = vm;
        _viewModelLoader = viewModelLoader;
		BindingContext = vm;
    }

    private async void CollectionView_Loaded(object sender, EventArgs e)
    {
        List<ZestawSearchEntryUI> zestawy = new();

        zestawy = await _viewModelLoader.LoadAllTests(true, true);

        viewModel.Zestawy = new ObservableCollection<ZestawSearchEntryUI>(zestawy);
    }
}