using FluentDataAccess.Core.Service;
using FluentDataAccess.Service;
using Wox.EasyHelper;
using Wox.EasyHelper.Core.Service;
using Wox.EasyHelper.Service;
using Wox.WebApp.Core.Service;
using Wox.WebApp.Service;

namespace Wox.WebApp
{
    public class WebAppPlugin : PluginBase<WebAppResultFinder>
    {
        public override WebAppResultFinder PrepareContext()
        {
            IQueryService queryService = new QueryService();
            IResultService resultService = new ResultService(WoxContextService);
            ISystemWebAppService systemWebAppService = new SystemWebAppService("Wox.WebApp");
            IDataAccessService dataAccessService = new DataAccessService(systemWebAppService);
            IWebAppItemRepository webAppItemRepository = new WebAppItemRepository(dataAccessService);
            IWebAppConfigurationRepository webAppConfigurationRepository = new WebAppConfigurationRepository(dataAccessService);
            IFileGeneratorService fileGeneratorService = new FileGeneratorService();
            IFileReaderService fileReaderService = new FileReaderService();
            IWebAppService webAppService = new WebAppService(dataAccessService, webAppItemRepository, webAppConfigurationRepository, systemWebAppService, fileGeneratorService, fileReaderService);
            WebAppResultFinder woxWebAppResultFinder = new WebAppResultFinder(WoxContextService, webAppService);

            webAppService.Init();
            woxWebAppResultFinder.Init();

            return woxWebAppResultFinder;
        }
    }
}