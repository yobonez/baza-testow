﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
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
        }
        [RelayCommand]
        void RemoveQuestion(PytanieUI question)
        {
            if (!Pytania.Contains(question))
                return;
            Pytania.Remove(question);
        }
        [RelayCommand]
        void ClearAll()
        {
            Zestaw = new ZestawUI();
            Pytania = new() { new PytanieUI() };
        }
    }
}
