using dominitian.Business.Contracts;
using dominitian.Business.Services;
using dominitian.Infrastructure.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace dominitian.Business.Tests.Services.Token
{
    [CollectionDefinition("DIServiceProviderCollection")]
    public class TestCollection : ICollectionFixture<ServiceProviderFixture>
    {
    }

    public class ServiceProviderFixture
    {
        public ServiceProvider ServiceProvider { get; private set; }

        public ServiceProviderFixture()
        {
            var serviceCollection = new ServiceCollection();
            var configurationManager = new ConfigurationManager();

            AddConfigManager(configurationManager);
            AddConfigurations(serviceCollection, configurationManager);
            AddServices(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        private void AddServices(IServiceCollection services)
        {
            services.AddSingleton<ITokenService, TokenService>();
        }

        private void AddConfigManager(ConfigurationManager manager)
        {
            manager.AddJsonFile("appsettings.json", optional:true, reloadOnChange: true);
        }

        private void AddConfigurations(IServiceCollection services, ConfigurationManager manager)
        {
            services.Configure<JwtTokenOptions>(manager.GetSection(JwtTokenOptions.SectionName));
        }
    }
}
