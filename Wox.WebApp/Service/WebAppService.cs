using FluentDataAccess.Core.Service;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Wox.EasyHelper.Core.Service;
using Wox.WebApp.Core.Service;
using Wox.WebApp.DomainModel;

namespace Wox.WebApp.Service
{
    public class WebAppService : IWebAppService
    {
        private IDataAccessService DataAccessService { get; set; }

        private IWebAppItemRepository WebAppItemRepository { get; set; }

        private IWebAppConfigurationRepository WebAppConfigurationRepository { get; set; }

        private ISystemWebAppService SystemService { get; set; }

        private IFileGeneratorService FileGeneratorService { get; set; }

        private IFileReaderService FileReaderService { get; set; }

        public WebAppService(IDataAccessService dataAccessService, IWebAppItemRepository webAppItemRepository, IWebAppConfigurationRepository webAppConfigurationRepository, ISystemWebAppService systemService, IFileGeneratorService fileGeneratorService, IFileReaderService fileReaderService)
        {
            DataAccessService = dataAccessService;
            WebAppItemRepository = webAppItemRepository;
            WebAppConfigurationRepository = webAppConfigurationRepository;
            SystemService = systemService;
            FileGeneratorService = fileGeneratorService;
            FileReaderService = fileReaderService;
        }

        public void Init()
        {
            DataAccessService.Init();
            WebAppItemRepository.Init();
            WebAppConfigurationRepository.Init();

            var configuration = WebAppConfigurationRepository.GetConfiguration();
            if (configuration == null)
            {
                configuration = new WebAppConfiguration()
                {
                    WebAppLauncher = "chrome.exe",
                    WebAppArgumentPattern = "--app=\"{0}\""
                };
                WebAppConfigurationRepository.SaveConfiguration(configuration);
            }
        }

        public void AddWebAppItem(string url, string keywords)
        {
            var item = new WebAppItem
            {
                Url = url,
                Keywords = keywords,
            };
            WebAppItemRepository.AddItem(item);
        }

        public IEnumerable<WebAppItem> Search(IEnumerable<string> terms)
        {
            return WebAppItemRepository.SearchItems(terms);
        }

        public void UpdateLauncher(string launcher, string argumentPattern)
        {
            var configuration = WebAppConfigurationRepository.GetConfiguration();
            if (configuration == null)
            {
                configuration = new WebAppConfiguration();
            }
            configuration.WebAppLauncher = launcher;
            configuration.WebAppArgumentPattern = argumentPattern;

            WebAppConfigurationRepository.SaveConfiguration(configuration);
        }

        public void StartUrl(string url)
        {
            var configuration = WebAppConfigurationRepository.GetConfiguration();
            var launcher = configuration.WebAppLauncher;
            var argumentsPattern = configuration.WebAppArgumentPattern;

            var arguments = string.Format(argumentsPattern, url);
            SystemService.StartCommandLine(launcher, arguments);
        }

        public WebAppConfiguration GetConfiguration() => WebAppConfigurationRepository.GetConfiguration();

        public void RemoveUrl(string url) => WebAppItemRepository.RemoveItem(url);

        public void Export()
        {
            var exportDirectory = SystemService.GetExportPath();
            var exportFilename = string.Format("Wox.WebApp-Save-{0}.wap.txt", SystemService.GetUID());
            var exportFullFilename = Path.Combine(exportDirectory, exportFilename);
            using (var fileGenerator = FileGeneratorService.CreateGenerator(exportFullFilename))
            {
                var configuration = GetConfiguration();
                fileGenerator.AddLine(string.Format("# launcher: {0}", configuration.WebAppLauncher));
                fileGenerator.AddLine(string.Format("# argumentsPattern: {0}", configuration.WebAppArgumentPattern));
                foreach (var webAppItem in WebAppItemRepository.SearchItems(new List<string>()))
                {
                    fileGenerator.AddLine(string.Format("{0} ({1})", webAppItem.Url, webAppItem.Keywords));
                }
                fileGenerator.Generate();
            }
            SystemService.StartCommandLine(exportDirectory, "");
        }

        public bool FileExists(string path) => FileReaderService.FileExists(path);

        public void Import(string path)
        {
            using (var fileReader = FileReaderService.Read(path))
            {
                var line = fileReader.ReadLine();
                while (line != null)
                {
                    line = line.Trim(' ', '\t', '\r', '\n');
                    if (line.StartsWith("#"))
                    {
                        if (line.Contains(":"))
                        {
                            var indexOfSeperater = line.IndexOf(":");
                            var key = line.Substring(0, indexOfSeperater).Trim(' ', '\t', '\r', '\n');
                            var value = line.Substring(indexOfSeperater + 1, line.Length - indexOfSeperater - 1).Trim(' ', '\t', '\r', '\n');
                            var configuration = GetConfiguration();
                            var changed = false;
                            if (key == "launcher")
                            {
                                configuration.WebAppLauncher = value;
                                changed = true;
                            }
                            if (key == "argumentsPattern")
                            {
                                configuration.WebAppArgumentPattern = value;
                                changed = true;
                            }
                            if (changed)
                            {
                                WebAppConfigurationRepository.SaveConfiguration(configuration);
                            }
                        }
                    }
                    else
                    {
                        var elements = line.Split(' ');
                        var url = elements[0];
                        var keywords = string.Join(" ", elements.Skip(1).Where(e => e.Length > 0).ToArray());
                        keywords = keywords.TrimStart('(', ' ', '\t', '\r', '\n').TrimEnd(')', ' ', '\t', '\r', '\n');
                        if (!string.IsNullOrEmpty(url))
                        {
                            AddWebAppItem(url, keywords);
                        }
                    }
                    line = fileReader.ReadLine();
                }
            }
        }

        public void Dispose()
        {
            DataAccessService.Dispose();
        }

        public WebAppItem GetUrlInfo(string url)
        {
            return WebAppItemRepository.GetItem(url);
        }

        public void EditWebAppItem(string url, string newUrl, string newKeywords)
        {
            WebAppItemRepository.EditWebAppItem(url, newUrl, newKeywords);
        }
    }
}