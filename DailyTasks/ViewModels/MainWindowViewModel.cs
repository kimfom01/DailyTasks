using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DailyTasks.Models;
using DailyTasks.Services;

namespace DailyTasks.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly IStorage<ToDoItem> _storage;

    public MainWindowViewModel(IStorage<ToDoItem> storage)
    {
        _storage = storage;
    }
    
    // TODO: Save changes when item is checked or unchecked. Look into ObservableCollection and find a way
    public ObservableCollection<ToDoItemViewModel> ToDoItems { get; private set; } = [];

    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(AddItemCommand))]
    private string? _newItemContent;

    private bool CanAddItem() => !string.IsNullOrWhiteSpace(NewItemContent);

    [RelayCommand(CanExecute = nameof(CanAddItem))]
    private async Task AddItem()
    {
        ToDoItems.Add(new ToDoItemViewModel
        {
            Content = NewItemContent
        });

        await _storage.SaveAsync(ToDoItems.Select(item => item.GetToDoItem()));

        NewItemContent = null;
    }

    [RelayCommand]
    private async Task DeleteItem(ToDoItemViewModel item)
    {
        ToDoItems.Remove(item);

        await _storage.SaveAsync(ToDoItems.Select(itemToSave => itemToSave.GetToDoItem()));
        var loadedItems = await _storage.LoadAsync();

        ToDoItems.Clear();
        foreach (var toDoItem in loadedItems)
        {
            ToDoItems.Add(new ToDoItemViewModel(toDoItem));
        }
    }
}