using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MyCompanionAI.Core;
using MyCompanionAI.Core.Interfaces;
using MyCompanionAI.Core.Services;
using MyCompanionAI.Data;
using OllamaSharp;

namespace MyCompanionAI;

public static class ConfigureServices
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => cfg.AddProfile<AutoMapperProfile>());
        return services;
    }
    public static IServiceCollection AddDataServices(this IServiceCollection services, ConfigurationManager config)
    {
        services.AddMemoryCache();
        services.AddDbContextFactory<MyCompanionDbContext>(opt =>
        {
            _ = opt.UseSqlServer(config.GetConnectionString("MyCompanionConnectionString"), sqlServerOptionsAction: sqlOptions =>
            {
                _ = sqlOptions.EnableRetryOnFailure();
                _ = sqlOptions.CommandTimeout(240);
            });
        }, ServiceLifetime.Scoped);

        services.AddScoped<IConversationService, ConversationService>();
        services.AddScoped<IChatService, ChatService>();

        services.AddHttpClient("MyCompanionAI", client =>
        {
            var url = $"{config["MyCompanionAI:BaseUrl"]}/api";
            client.BaseAddress = new Uri(url);
        });
        return services;
    }

    public static IServiceCollection AddAIServices(this IServiceCollection services, ConfigurationManager config)
    {
        var url = $"{config["MyCompanionAI:BaseUrl"]}";
        var model = $"{config["MyCompanionAI:Model"]}";

        services.AddSingleton<IChatClient>(new OllamaApiClient(url)
        {
            SelectedModel = model
        });

        return services;
    }
}
