using System.Linq;
using Wox.WebApp.Core.Service;
using Wox.WebApp.DomainModel;

namespace Wox.WebApp.Service
{
    public class WebAppConfigurationRepository : IWebAppConfigurationRepository
    {
        private IDataAccessService DataAccessService { get; set; }

        public WebAppConfigurationRepository(IDataAccessService dataAccessService)
        {
            DataAccessService = dataAccessService;
        }

        public void Init()
        {
            DataAccessService
                .GetQuery("create table if not exists configuration (id integer primary key, launcher text, pattern text);")
                .Execute();
        }

        public WebAppConfiguration GetConfiguration()
        {
            var configurations = DataAccessService
                .GetQuery("select launcher, pattern from configuration;")
                .Returning<WebAppConfiguration>()
                .Reading("launcher", (WebAppConfiguration configuration, string value) => configuration.WebAppLauncher = value)
                .Reading("pattern", (WebAppConfiguration configuration, string value) => configuration.WebAppArgumentPattern = value)
                .Execute();
            if (configurations.Any())
            {
                return configurations.First();
            }
            return null;
        }

        public void SaveConfiguration(WebAppConfiguration configuration)
        {
            var affectedRows = DataAccessService
                .GetQuery("update configuration set launcher=@launcher, pattern=@pattern")
                .WithParameter("launcher", configuration.WebAppLauncher)
                .WithParameter("pattern", configuration.WebAppArgumentPattern)
                .Execute();
            if (affectedRows == 0)
            {
                DataAccessService
                    .GetQuery("insert into configuration (launcher, pattern) values (@launcher, @pattern)")
                    .WithParameter("launcher", configuration.WebAppLauncher)
                    .WithParameter("pattern", configuration.WebAppArgumentPattern)
                    .Execute();
            }
        }
    }
}