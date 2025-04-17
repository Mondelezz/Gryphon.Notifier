using Application.Interfaces;
using Application.Services;

using FluentValidation;

using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection RegisterApplicationLayer(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining(typeof(DependencyInjection), ServiceLifetime.Transient, includeInternalTypes: true);

        RegisteringServices(services);

        return services;
    }

    private static void RegisteringServices(IServiceCollection services)
    {
        services.AddScoped<IFileDataService, FileDataService>();
        services.AddScoped<ITopicService, TopicService>();
        services.AddScoped<IEventService, EventService>();
        services.AddScoped<IAuthorizationService, AuthorizationService>();
    }
}
