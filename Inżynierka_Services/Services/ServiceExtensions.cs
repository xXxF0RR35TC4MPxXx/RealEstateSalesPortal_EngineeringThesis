using Inżynierka_Common.ServiceRegistrationAttributes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Inżynierka_Services.Services
{
    public static class ServiceExtensions
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {

            Type scopedRegistration = typeof(ScopedRegistrationAttribute);
            Type scopedRegistrationWithInterface = typeof(ScopedRegistrationWithInterfaceAttribute);

            var types = AppDomain.CurrentDomain.GetAssemblies()
             .SelectMany(s => s.GetTypes())
             .Where(p => p.IsDefined(scopedRegistration, true) && !p.IsInterface).Select(s => new
             {
                 Implementation = s
             });


            var types2 = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => p.IsDefined(scopedRegistrationWithInterface, true) && !p.IsInterface).Select(s => new
            {
                Service = s.GetInterface($"I{s.Name}"),
                Implementation = s
            }).Where(x => x.Service != null);


            foreach (var type in types)
            {
                if (type.Implementation.IsDefined(scopedRegistration, false))
                {
                    services.AddScoped(type.Implementation);
                }
            }

            foreach (var type in types2)
            {
                if (type.Implementation.IsDefined(scopedRegistrationWithInterface, false))
                {
                    services.AddScoped(type.Service, type.Implementation);
                }
            }
        }
    }
}
