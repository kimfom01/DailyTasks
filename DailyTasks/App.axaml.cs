using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using DailyTasks.Extensions;
using DailyTasks.Models;
using DailyTasks.Services;
using DailyTasks.ViewModels;
using DailyTasks.Views;
using Microsoft.Extensions.DependencyInjection;

namespace DailyTasks;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override async void OnFrameworkInitializationCompleted()
    {
        var collection = new ServiceCollection();
        collection.RegisterServices();

        var serviceProvider = collection.BuildServiceProvider();

        var mainWindowViewModel = serviceProvider.GetRequiredService<MainWindowViewModel>();
        var storage = serviceProvider.GetRequiredService<IStorage<ToDoItem>>();
        
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Line below is needed to remove Avalonia data validation.
            // Without this line you will get duplicate validations from both Avalonia and CT
            BindingPlugins.DataValidators.RemoveAt(0);
            desktop.MainWindow = new MainWindow
            {
                DataContext = mainWindowViewModel,
            };

            var loadedItems = await storage.LoadAsync();
            foreach (var item in loadedItems)
            {
                mainWindowViewModel.ToDoItems.Add(new ToDoItemViewModel(item));
            }
        }

        base.OnFrameworkInitializationCompleted();
    }
}