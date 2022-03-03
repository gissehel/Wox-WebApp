using System.Collections.Generic;
using Wox.WebApp.DomainModel;

namespace Wox.WebApp.Core.Service
{
    public interface IWebAppConfigurationRepository
    {
        void Init();

        IEnumerable<WebAppConfiguration> GetConfigurations();

        WebAppConfiguration GetConfiguration(string profile);

        void SaveConfiguration(WebAppConfiguration configuration);
    }
}