using AutoMapper;
using HomeQI.ADream.Identity.ViewModels;
namespace Microsoft.Extensions.DependencyInjection
{
    public static class IdentityViewModelsAutoMapperEx
    {
        public static IServiceCollection AddIdentityModelsAutoMapper(this IServiceCollection services)
        {
            IConfigurationProvider config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<IdentityMappingProfile>();
            });
            services.AddSingleton(config);
            services.AddScoped<IMapper, Mapper>();

            return services;
        }
    }
}
