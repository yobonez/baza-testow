using TestyMAUI.ViewModel;

namespace TestyMAUI;

public partial class TestSelectorPage : ContentPage
{
	public TestSelectorPage(TestSelectorViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}