using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Plugin.LocalNotification;
using Plugin.LocalNotification.AndroidOption;
using TodoApp.Application.cs.Contracts;
using TodoApp.Application.cs.Todos;
using TodoApp.Infrastructure.Context;
using TodoApp.Infrastructure.Repositories;

namespace TodoApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .UseLocalNotification();

            builder.Services.AddSingleton<TodoContext>();
            builder.Services.AddSingleton<ITodoService, TodoService>();
            builder.Services.AddSingleton<ITodoRepository, TodoRepository>();
#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
