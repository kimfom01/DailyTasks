using DailyTasks.Services;
using DailyTasks.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace DailyTasks.Extensions;

public static class ServiceCollections
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddSingleton(typeof(IStorage<>), typeof(FileStorage<>));
        services.AddTransient<MainWindowViewModel>();
    }
}