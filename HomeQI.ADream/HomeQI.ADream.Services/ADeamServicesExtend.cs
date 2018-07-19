using HomeQI.ADream.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ADeamServicesExtend
    {
        public static IServiceCollection AddADeamServices(this IServiceCollection serviceCollection, Action action = null)
        {
            var configuration = serviceCollection.BuildServiceProvider().GetRequiredService<IConfiguration>();
            serviceCollection.Configure<SmsConfig>(configuration.GetSection("SmsConfig"));
            serviceCollection.Configure<MailConfig>(configuration.GetSection("MailConfigs"));
            serviceCollection.TryAddScoped<ISmsSender, MessageSender>();
            serviceCollection.TryAddScoped<IEmailSender, MessageSender>();
            serviceCollection.TryAddScoped<TokenGenerater>();
            serviceCollection.TryAddScoped<MessageSender>(); serviceCollection.AddHttpClient();
            action?.Invoke();
            return serviceCollection;
        }
    }
}
