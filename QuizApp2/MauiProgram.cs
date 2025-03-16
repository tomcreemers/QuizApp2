using Microsoft.Extensions.Logging;
using QuizApp2.Repositories;
using QuizApp2.Models;
using QuizApp2.Services;

namespace QuizApp2
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });
                
                


            // Register repositories
            builder.Services.AddSingleton<GenericRepository<User>>();
            builder.Services.AddSingleton<GenericRepository<QuizQuestion>>();

            // Register the API service
            builder.Services.AddSingleton<JokeApiService>();
            builder.Services.AddSingleton<GenericRepository<Session>>();
            builder.Services.AddSingleton<GenericRepository<SessionParticipant>>();
            builder.Services.AddSingleton<GenericRepository<SessionMessage>>();

#if DEBUG
            builder.Logging.AddDebug();
#endif
            return builder.Build();
        }
    }
}
