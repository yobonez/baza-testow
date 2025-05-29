using TestyMAUI.ViewModel;

namespace TestyMAUI;

public partial class QuestionsCreatorPage : ContentPage
{
    private QuestionsCreatorViewModel viewModel;
	public QuestionsCreatorPage(QuestionsCreatorViewModel vm)
	{
		InitializeComponent();
        viewModel = vm;
		BindingContext = vm;
    }

    private async void HorizontalStackLayout_Loaded(object sender, EventArgs e)
    {
        await viewModel.LoadSubjectsNCategories();
    }

    private void GetFromDb_Clicked(object sender, EventArgs e)
    {
        
    }
}