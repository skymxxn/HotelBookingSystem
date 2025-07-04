using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace HotelBookingSystem.Application.DependencyInjection;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(ApplicationServiceRegistration).Assembly;
        
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(assembly));
        
        services.AddValidatorsFromAssembly(assembly);

        return services;
    }
}