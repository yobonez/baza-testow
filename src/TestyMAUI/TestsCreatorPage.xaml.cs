using TestyMAUI.ViewModel;

namespace TestyMAUI;

public partial class TestsCreatorPage : ContentPage
{
    private TestsCreatorViewModel viewModel;

    public TestsCreatorPage(TestsCreatorViewModel vm)
	{
		InitializeComponent();
		viewModel = vm;
        BindingContext = vm;
	}

    private async void HorizontalStackLayout_Loaded(object sender, EventArgs e)
    {
        await viewModel.LoadSubjects();
    }
}