using CommunityToolkit.Maui;
using InputKit.Shared.Controls;
using Mopups.Hosting;
using TodoApp.Application.cs.Contracts;
using TodoApp.Application.cs.Todos;
using TodoApp.Infrastructure.Context;
using TodoApp.Infrastructure.Repositories;
using UraniumUI;

namespace TodoApp.Uranium
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureMopups()
                .UseUraniumUI()
                .UseUraniumUIMaterial()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");

                    //fonts.AddFontAwesomeIconFonts();
                    fonts.AddMaterialIconFonts();
                });

            builder.Services.AddMopupsDialogs();
            builder.Services.AddSingleton<TodoContext>();
            builder.Services.AddSingleton<ITodoService, TodoService>();
            builder.Services.AddSingleton<ITodoRepository, TodoRepository>();
            return builder.Build();
        }
    }
}
