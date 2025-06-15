using HotelBookingSystem.Application.Common;
using Microsoft.Extensions.DependencyInjection;

namespace HotelBookingSystem.Application.DependencyInjection;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(ApplicationServiceRegistration).Assembly));

        return services;
    }
}