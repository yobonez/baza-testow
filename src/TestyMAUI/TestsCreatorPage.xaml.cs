using TestyMAUI.ViewModel;

namespace TestyMAUI;

public partial class TestsCreatorPage : ContentPage
{
	public TestsCreatorPage(TestsCreatorViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}