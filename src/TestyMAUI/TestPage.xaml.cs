using CommunityToolkit.Mvvm.ComponentModel;
using System.Threading.Tasks;
using TestyMAUI.UIModels;
using TestyMAUI.ViewModel;

namespace TestyMAUI;

[QueryProperty(nameof(LoadedTest), "test")]
public partial class TestPage : ContentPage, IQueryAttributable
{
    private TestViewModel viewModel;

    ZestawSearchEntryUI LoadedTest { get; set; }
	public TestPage(TestViewModel vm)
	{
		InitializeComponent();
		viewModel = vm;
        BindingContext = vm;
	}

    private void ContentPage_Loaded(object sender, EventArgs e)
    {
        viewModel.TheTest = LoadedTest;
        viewModel.InitializeTest();
    }
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("test", out var test))
            LoadedTest = test as ZestawSearchEntryUI;
    }
}