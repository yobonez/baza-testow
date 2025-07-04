using TestyMAUI.ViewModel;

namespace TestyMAUI;
[QueryProperty(nameof(IsFullQuestion), "isFullQuestion")]
[QueryProperty(nameof(SubjectFilter), "subjectFilter")]
public partial class SearchPage : ContentPage, IQueryAttributable
{
	private SearchViewModel viewModel;

    string IsFullQuestion { get; set; }
    string? SubjectFilter { get; set; }


    public SearchPage(SearchViewModel vm)
	{
		InitializeComponent();
		viewModel = vm;
		BindingContext = vm;
	}

    private async void ContentPage_Loaded(object sender, EventArgs e)
    {
        viewModel._isFullQuestion = bool.Parse(IsFullQuestion);
		await viewModel.LoadAllQuestions(SubjectFilter);
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("isFullQuestion", out var isFullQuestion)) {
            IsFullQuestion = isFullQuestion as string;
        }
        if (query.TryGetValue("subjectFilter", out var subjectFilter)) {
            SubjectFilter = subjectFilter as string;
        }
    }
}