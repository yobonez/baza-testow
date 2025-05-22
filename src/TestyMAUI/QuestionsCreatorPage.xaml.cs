using TestyMAUI.ViewModel;

namespace TestyMAUI;

public partial class QuestionsCreatorPage : ContentPage
{
	public QuestionsCreatorPage(QuestionsCreatorViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}