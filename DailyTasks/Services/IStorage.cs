using System.Collections.Generic;
using System.Threading.Tasks;

namespace DailyTasks.Services;

public interface IStorage<T>
{
    Task SaveAsync(IEnumerable<T> items);
    Task<IEnumerable<T>> LoadAsync();
}