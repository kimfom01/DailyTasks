using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DailyTasks.Models;

namespace DailyTasks.ViewModels;

public partial class ToDoItemViewModel : ViewModelBase
{
    [ObservableProperty] private bool _isChecked;

    [ObservableProperty] private string? _content;

    public ToDoItemViewModel()
    {
    }

    public ToDoItemViewModel(ToDoItem item)
    {
        IsChecked = item.IsChecked;
        Content = item.Content;
    }

    public ToDoItem GetToDoItem() => new()
    {
        IsChecked = IsChecked,
        Content = Content
    };

    [RelayCommand]
    private async Task Copy(string? content)
    {
        if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop
            || desktop.MainWindow?.Clipboard is not { } provider)
            throw new NullReferenceException("Missing Clipboard Instance");

        await provider.SetTextAsync(content);
    }
}