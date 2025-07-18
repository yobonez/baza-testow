using CommunityToolkit.Mvvm.Messaging;
using Maui.DataGrid;
using System.ComponentModel.Design;
using TestyMAUI.Messages;
using TestyMAUI.UIModels;
using TestyMAUI.ViewModel;

namespace TestyMAUI;

[QueryProperty(nameof(IsFullQuestion), "isFullQuestion")]
[QueryProperty(nameof(SubjectFilter), "subjectFilter")]
[QueryProperty(nameof(IsTestSearch), "isTestSearch")]
public partial class SearchPage : ContentPage, IQueryAttributable
{
	private SearchViewModel viewModel;

    string IsFullQuestion { get; set; }
    string? SubjectFilter { get; set; }
    string? IsTestSearch { get; set; }


    public SearchPage(SearchViewModel vm)
	{
		InitializeComponent();
		viewModel = vm;
		BindingContext = vm;
	}

    private async void ContentPage_Loaded(object sender, EventArgs e)
    {
        viewModel.ResetSelection();
        viewModel._isFullQuestion = bool.Parse(IsFullQuestion);
        viewModel._isTestSearch = bool.Parse(IsTestSearch ?? "False");

        await viewModel.LoadAllItems(SubjectFilter);

        loadingLabel.Text = "�adowanie...";

        if (viewModel._isTestSearch)
        {
            searchContents.SetBinding(DataGrid.ItemsSourceProperty, nameof(viewModel.Zestawy));
            searchContents.SetBinding(DataGrid.SelectedItemProperty, nameof(viewModel.WybranyZestaw));
        }
        else
        {
            searchContents.SetBinding(DataGrid.ItemsSourceProperty, nameof(viewModel.Pytania));
            searchContents.SetBinding(DataGrid.SelectedItemProperty, nameof(viewModel.WybranePytanie));
        }

        searchContents.SetBinding(DataGrid.ColumnsProperty, nameof(viewModel.Columns));

        loadingLabel.Text = "Brak danych.";
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("isFullQuestion", out var isFullQuestion))
            IsFullQuestion = isFullQuestion as string;

        if (query.TryGetValue("subjectFilter", out var subjectFilter))
        { SubjectFilter = Uri.UnescapeDataString((string)subjectFilter); }

        if (query.TryGetValue("isTestSearch", out var isTestSearch))
            IsTestSearch = isTestSearch as string;
    }

    protected override void OnNavigatingFrom(NavigatingFromEventArgs args)
    {
        base.OnNavigatingFrom(args);
        if (!viewModel.hasItemBeenSelected)
        {
            if (!viewModel._isTestSearch)
            {
                if (viewModel._isFullQuestion)
                    WeakReferenceMessenger.Default.Send<GetDetailedQuestionMessage>(new GetDetailedQuestionMessage(null!));
                else
                    WeakReferenceMessenger.Default.Send<GetSimpleQuestionMessage>(new GetSimpleQuestionMessage(null!));
            }
            else WeakReferenceMessenger.Default.Send<GetTestMessage>(new GetTestMessage(null!));
        }
    }
}