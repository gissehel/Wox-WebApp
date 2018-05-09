using Wox.WebApp.DomainModel;

namespace Wox.WebApp.Core.Service
{
    public interface IWebAppConfigurationRepository
    {
        void Init();

        WebAppConfiguration GetConfiguration();

        void SaveConfiguration(WebAppConfiguration configuration);
    }
}