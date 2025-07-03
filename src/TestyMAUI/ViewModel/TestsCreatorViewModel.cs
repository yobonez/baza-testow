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
            Pytania = new() { new PytanieUI() };
        }

        [ObservableProperty]
        ZestawUI zestaw;

        [ObservableProperty]
        ObservableCollection<PytanieUI> pytania;

        [RelayCommand]
        void AddQuestion()
        {
            Pytania.Add(new PytanieUI());
            RefreshQuestionIndexes();
        }
        [RelayCommand]
        void RemoveQuestion(PytanieUI question)
        {
            if (!Pytania.Contains(question) || Pytania.Count == 1)
                return;
            Pytania.Remove(question);
            RefreshQuestionIndexes();
        }
        [RelayCommand]
        void ClearAll()
        {
            Zestaw = new ZestawUI();
            Pytania = new() { new PytanieUI() };
            RefreshQuestionIndexes();
        }

        // TODO: to-dry
        private void RefreshQuestionIndexes()
        {
            int counter = 1;
            foreach (PytanieUI pyt in Pytania)
            {
                pyt.Idx = "Pytanie " + counter;
                counter++;
            }
        }
    }
}
