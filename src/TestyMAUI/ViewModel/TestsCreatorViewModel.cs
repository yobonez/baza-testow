using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TestyLogic.Models;
using TestyMAUI.UIModels;

namespace TestyMAUI.ViewModel
{
    public partial class TestsCreatorViewModel : ObservableObject
    {
        public TestsCreatorViewModel()
        {
            Zestaw = new() { }; 
            Pytania = new()
            {
            };
        }

        [ObservableProperty]
        ZestawUI zestaw;

        [ObservableProperty]
        ObservableCollection<PytanieUI> pytania;

        [RelayCommand]
        void AddQuestion()
        {
            Pytania.Add(new PytanieUI(0, "", 0, false));
        }
        [RelayCommand]
        void RemoveQuestion(PytanieUI question)
        {
            if (Pytania.Contains(question)) Pytania.Remove(question);
        }
        [RelayCommand]
        void ClearAll()
        {
            Zestaw = new ZestawUI();
            Pytania = new() { new PytanieUI(0, "", 0, false) };
        }
    }
}
