using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using DailyTasks.Models;

namespace DailyTasks.Services;

public static class ToDoListFileService
{
    private static readonly string JsonPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
        "DailyTasks", "database.json"
    );

    public static async Task SaveToFileAsync(IEnumerable<ToDoItem> itemsToSave)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(JsonPath)!);

        await using var fileSystem = File.Create(JsonPath);

        await JsonSerializer.SerializeAsync(fileSystem, itemsToSave);
    }

    public static async Task<IEnumerable<ToDoItem>> LoadFromFileAsync()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(JsonPath)!);

        try
        {
            await using var fileSystem = File.OpenRead(JsonPath);

            var todoItems = await JsonSerializer.DeserializeAsync<IEnumerable<ToDoItem>>(fileSystem);

            return todoItems ?? [];
        }
        catch (JsonException)
        {
            return [];
        }
    }
}