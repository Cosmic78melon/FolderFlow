using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using FileOrganizer.Backend_Services;
using FileOrganizer.ViewModels;
using FileOrganizer.Views;
using Microsoft.Extensions.DependencyInjection;

namespace FileOrganizer;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var collections = new ServiceCollection();
        collections.AddSingleton<MainWindowViewModel>();
        collections.AddSingleton<HomePageViewModel>();
        collections.AddSingleton<SettingsPageViewModel>();
        collections.AddSingleton<IOrganizer, Organizer>();
        collections.AddSingleton<FilePicker>();
        collections.AddSingleton<Func<TopLevel?>>(_ => () =>
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime window)
            {
                return TopLevel.GetTopLevel(window.MainWindow);
            }

            return null;
        });
        
        var services = collections.BuildServiceProvider();
        
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = services.GetRequiredService<MainWindowViewModel>()
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}