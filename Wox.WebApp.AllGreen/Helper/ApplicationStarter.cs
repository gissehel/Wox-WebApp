using System;
using System.IO;
using System.Reflection;
using Wox.WebApp.Core.Service;
using Wox.WebApp.Mock.Service;
using Wox.WebApp.Service;

namespace Wox.WebApp.AllGreen.Helper
{
    public class ApplicationStarter
    {
        public WoxContextServiceMock WoxContextService { get; set; }
        public QueryServiceMock QueryService { get; set; }
        public SystemServiceMock SystemService { get; set; }
        public IWoxResultFinder WoxWebAppResultFinder { get; set; }
        private IWebAppService WebAppService { get; set; }
        public FileGeneratorServiceMock FileGeneratorService { get; set; }
        private string TestName { get; set; }

        public string TestPath => SystemService.ApplicationDataPath;

        public void Init(string testName)
        {
            TestName = testName;
            QueryServiceMock queryService = new QueryServiceMock();
            WoxContextServiceMock woxContextService = new WoxContextServiceMock(queryService);
            SystemServiceMock systemService = new SystemServiceMock();
            IDataAccessService dataAccessService = new DataAccessService(systemService);
            IWebAppItemRepository webAppItemRepository = new WebAppItemRepository(dataAccessService);
            IWebAppConfigurationRepository webAppConfigurationRepository = new WebAppConfigurationRepository(dataAccessService);
            FileGeneratorServiceMock fileGeneratorService = new FileGeneratorServiceMock();
            IWebAppService webAppService = new WebAppService(dataAccessService, webAppItemRepository, webAppConfigurationRepository, systemService, fileGeneratorService);
            IWoxResultFinder woxWebAppResultFinder = new WebAppResultFinder(woxContextService, webAppService);

            systemService.ApplicationDataPath = GetApplicationDataPath();

            WoxContextService = woxContextService;
            QueryService = queryService;
            SystemService = systemService;
            WebAppService = webAppService;
            FileGeneratorService = fileGeneratorService;
            WoxWebAppResultFinder = woxWebAppResultFinder;

            WoxContextService.AddQueryFetcher("wap", WoxWebAppResultFinder);
        }

        public void Start()
        {
            WebAppService.Init();
        }

        private static string GetThisAssemblyDirectory()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var thisAssemblyCodeBase = assembly.CodeBase;
            var thisAssemblyDirectory = Path.GetDirectoryName(new Uri(thisAssemblyCodeBase).LocalPath);

            return thisAssemblyDirectory;
        }

        private string GetApplicationDataPath()
        {
            var thisAssemblyDirectory = GetThisAssemblyDirectory();
            var path = Path.Combine(Path.Combine(thisAssemblyDirectory, "AllGreen"), string.Format("AG_{0:yyyyMMdd-HHmmss-fff}_{1}", DateTime.Now, TestName));
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }
    }
}