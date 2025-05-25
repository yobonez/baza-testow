using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TestyMAUI.UIModels;

namespace TestyMAUI.ViewModel
{
    public partial class QuestionsCreatorViewModel : ObservableObject
    {
        public QuestionsCreatorViewModel() 
        {
            Pytanie = new PytanieUI(0, "", 1, false);
            Odpowiedzi = new()
            {
                new OdpowiedzUI(0,"", true, 0)
            };
            Przedmioty = new();
            Kategorie = new();
        }

        [ObservableProperty]
        PytanieUI pytanie;

        [ObservableProperty]
        ObservableCollection<KategoriaUI> kategorie;

        [ObservableProperty]
        ObservableCollection<PrzedmiotUI> przedmioty;

        [ObservableProperty]
        ObservableCollection<OdpowiedzUI> odpowiedzi;

        [RelayCommand]
        void AddAnswer()
        {
            Odpowiedzi.Add(new OdpowiedzUI(0, "", false, 0));
        }
        [RelayCommand]
        void RemoveAnswer(OdpowiedzUI answer)
        {
            if (Odpowiedzi.Contains(answer)) Odpowiedzi.Remove(answer);
        }
        [RelayCommand]
        void ClearAll()
        {
            Pytanie = new PytanieUI(0, "", 0, false);
            Odpowiedzi = new ObservableCollection<OdpowiedzUI>() { new OdpowiedzUI(0, "", false, 0) };
        }
    }
}
