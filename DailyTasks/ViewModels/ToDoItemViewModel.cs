using CommunityToolkit.Mvvm.ComponentModel;
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
}