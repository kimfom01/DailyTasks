using System.Linq;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using DailyTasks.Services;
using DailyTasks.ViewModels;
using DailyTasks.Views;

namespace DailyTasks;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private readonly MainWindowViewModel _mainWindowViewModel = new();

    public override async void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Line below is needed to remove Avalonia data validation.
            // Without this line you will get duplicate validations from both Avalonia and CT
            BindingPlugins.DataValidators.RemoveAt(0);
            desktop.MainWindow = new MainWindow
            {
                DataContext = _mainWindowViewModel,
            };

            desktop.ShutdownRequested += DesktopOnShutDownRequested;

            var loadedItems = await ToDoListFileService.LoadFromFileAsync();
            foreach (var item in loadedItems)
            {
                _mainWindowViewModel.ToDoItems.Add(new ToDoItemViewModel(item));
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
            var itemsToSave = _mainWindowViewModel.ToDoItems.Select(item => item.GetToDoItem());
            await ToDoListFileService.SaveToFileAsync(itemsToSave);

            _canClose = true;
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.Shutdown();
            }
        }
    }
}