﻿using TestyMAUI.ViewModel;

namespace TestyMAUI;

public partial class MainPage : ContentPage
{
    public MainPage(MainViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
