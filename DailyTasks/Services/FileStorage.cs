using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace DailyTasks.Services;

public class FileStorage<TItem> : IStorage<TItem>
{
    private static readonly string JsonPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
        "DailyTasks", "database.json"
    );

    public async Task SaveAsync(IEnumerable<TItem> items)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(JsonPath)!);

        await using var fileSystem = File.Create(JsonPath);

        await JsonSerializer.SerializeAsync(fileSystem, items);
    }

    public async Task<IEnumerable<TItem>> LoadAsync()
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(JsonPath)!);
     
            await using var fileSystem = File.OpenRead(JsonPath);

            var todoItems = await JsonSerializer.DeserializeAsync<IEnumerable<TItem>>(fileSystem);

            return todoItems ?? [];
        }
        catch (Exception)
        {
            return [];
        }
    }
}