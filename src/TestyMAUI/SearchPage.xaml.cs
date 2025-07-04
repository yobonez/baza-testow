using TestyMAUI.ViewModel;

namespace TestyMAUI;
[QueryProperty(nameof(IsFullQuestion), "isFullQuestion")]
public partial class SearchPage : ContentPage, IQueryAttributable
{
	private SearchViewModel viewModel;

    string IsFullQuestion { get; set; }


    public SearchPage(SearchViewModel vm)
	{
		InitializeComponent();
		viewModel = vm;
		BindingContext = vm;
	}

    private async void ContentPage_Loaded(object sender, EventArgs e)
    {
        viewModel._isFullQuestion = bool.Parse(IsFullQuestion);
		await viewModel.LoadAllQuestions();
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("isFullQuestion", out var value)) {
            IsFullQuestion = value as string;
        }
    }
}