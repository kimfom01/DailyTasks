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

            desktop.ShutdownRequested += DesktopOnShutDownRequested;

            var loadedItems = await storage.LoadAsync();
            foreach (var item in loadedItems)
            {
                mainWindowViewModel.ToDoItems.Add(new ToDoItemViewModel(item));
            }
        }

        base.OnFrameworkInitializationCompleted();
    }

    private bool _canClose;
    private async void DesktopOnShutDownRequested(object? sender, ShutdownRequestedEventArgs e)
    {
        e.Cancel = !_canClose;

        if (!_canClose)
        {
            // TODO: Move logic to where change actually occured, use DI to inject services
            // var itemsToSave = _mainWindowViewModel.ToDoItems.Select(item => item.GetToDoItem());
            // await FileStorageService.SaveToFileAsync(itemsToSave);

            _canClose = true;
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.Shutdown();
            }
        }
    }
}