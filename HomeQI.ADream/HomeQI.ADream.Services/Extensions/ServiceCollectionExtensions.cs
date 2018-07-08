using HomeQI.ADream.IServcies;
using Microsoft.Extensions.DependencyInjection;

namespace HomeQI.ADream.Services.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddADreamService(this IServiceCollection serviceDescriptors)
        {
            serviceDescriptors.AddScoped<IServiceFactory, ServiceFactory>();
            return serviceDescriptors;
        }
    }
}
