using TestyMAUI.ViewModel;

namespace TestyMAUI;

public partial class SearchPage : ContentPage
{
	private SearchViewModel viewModel;
	public SearchPage(SearchViewModel vm)
	{
		InitializeComponent();
		viewModel = vm;
		BindingContext = vm;
	}

    private async void ContentPage_Loaded(object sender, EventArgs e)
    {
		await viewModel.LoadAllQuestions();
    }
}