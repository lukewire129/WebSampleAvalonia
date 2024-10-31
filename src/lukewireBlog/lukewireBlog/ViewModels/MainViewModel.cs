﻿using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Windows.Input;
using lukewireBlog.Domain.Main.Models;
using ReactiveUI;

namespace lukewireBlog.ViewModels;

public partial class MainViewModel : ReactiveObject
{
    private ViewModelBase[] Pages;
    private ViewModelBase _CurrentPage;

    public ViewModelBase CurrentPage
    {
        get { return _CurrentPage; }
        set { this.RaiseAndSetIfChanged(ref _CurrentPage, value); }
    }

    public List<TapMenuModel> SideMenus { get; set; } = new()
    {
        new TapMenuModel(0, "NavigationBar"),
        new TapMenuModel(1, "NavigationView"),  
        new TapMenuModel(2, "RiotPlayButton"),
        new TapMenuModel(3, "DatePickerStyle"),
        new TapMenuModel(4, "SocialICon"),
        new TapMenuModel(5, "ThemeSwitch"),
    };

    public ICommand NavigateCommand { get; }

    public MainViewModel()
    {
        Pages = new ViewModelBase[]
        {
            new NavigationBarViewModel(),
            new NavigationViewViewModel(),
            new RiotPlayButtonViewModel(),
            new SmartDateViewModel(),
            new SocialIconViewModel(),
            new ThemeSwitchViewModel()
        };

        Pages[0].Load();
        CurrentPage = Pages[0];

        NavigateCommand = ReactiveCommand.Create<TapMenuModel>(async (model) =>
        {
            var page = Pages[model.Idx];
            RxApp.MainThreadScheduler.Schedule(() =>
            {
                CurrentPage = page;
            });
        }, outputScheduler: RxApp.TaskpoolScheduler);
    }
}