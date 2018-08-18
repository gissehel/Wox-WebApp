using FluentDataAccess.Core.Service;
using FluentDataAccess.Service;
using System.Collections.Generic;
using Wox.EasyHelper.Core.Service;
using Wox.EasyHelper.Service;
using Wox.Plugin;
using Wox.WebApp.Core.Service;
using Wox.WebApp.Service;

namespace Wox.WebApp
{
    public class WebAppPlugin : IPlugin
    {
        private IQueryService QueryService { get; set; }
        private IResultService ResultService { get; set; }

        private IWoxResultFinder WoxWebAppResultFinder { get; set; }

        public void Init(PluginInitContext context)
        {
            IWoxContextService woxContextService = new WoxContextService(context);
            IQueryService queryService = new QueryService();
            IResultService resultService = new ResultService(woxContextService);
            ISystemWebAppService systemWebAppService = new SystemWebAppService("Wox.WebApp");
            IDataAccessService dataAccessService = new DataAccessService(systemWebAppService);
            IWebAppItemRepository webAppItemRepository = new WebAppItemRepository(dataAccessService);
            IWebAppConfigurationRepository webAppConfigurationRepository = new WebAppConfigurationRepository(dataAccessService);
            IFileGeneratorService fileGeneratorService = new FileGeneratorService();
            IFileReaderService fileReaderService = new FileReaderService();
            IWebAppService webAppService = new WebAppService(dataAccessService, webAppItemRepository, webAppConfigurationRepository, systemWebAppService, fileGeneratorService, fileReaderService);
            IWoxResultFinder woxWebAppResultFinder = new WebAppResultFinder(woxContextService, webAppService);

            webAppService.Init();

            QueryService = queryService;
            ResultService = resultService;
            WoxWebAppResultFinder = woxWebAppResultFinder;
        }

        public List<Result> Query(Query query)
        {
            var woxQuery = QueryService.GetWoxQuery(query);
            var results = WoxWebAppResultFinder.GetResults(woxQuery);
            return ResultService.MapResults(results);
        }
    }
}