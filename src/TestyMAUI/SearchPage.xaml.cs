using TestyMAUI.ViewModel;

namespace TestyMAUI;

public partial class SearchPage : ContentPage
{
	public SearchPage(SearchViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}