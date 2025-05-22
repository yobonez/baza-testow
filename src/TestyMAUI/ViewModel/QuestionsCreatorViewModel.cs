using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TestyLogic.Models;

namespace TestyMAUI.ViewModel
{
    public partial class QuestionsCreatorViewModel : ObservableObject
    {
        [ObservableProperty]
        Pytanie pytanie;

        [ObservableProperty]
        ObservableCollection<Odpowiedz> odpowiedzi;

        [RelayCommand]
        void AddAnswer()
        {

        }
        [RelayCommand]
        void RemoveAnswer()
        {

        }
    }
}
