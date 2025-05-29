namespace TestyMAUI
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(QuestionsCreatorPage), typeof(QuestionsCreatorPage));
            Routing.RegisterRoute(nameof(TestsCreatorPage), typeof(TestsCreatorPage));
            Routing.RegisterRoute(nameof(TestSelectorPage), typeof(TestSelectorPage));
            Routing.RegisterRoute(nameof(SearchPage), typeof(SearchPage));
        }
    }
}
