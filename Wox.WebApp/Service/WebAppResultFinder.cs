using System;
using System.Collections.Generic;
using System.Linq;
using Wox.WebApp.Core.Service;
using Wox.WebApp.DomainModel;

namespace Wox.WebApp.Service
{
    public class WebAppResultFinder : IWoxResultFinder
    {
        private IWoxContextService WoxContextService { get; set; }
        private IWebAppService WebAppService { get; set; }

        public WebAppResultFinder(IWoxContextService woxContextService, IWebAppService webAppService)
        {
            WoxContextService = woxContextService;
            WebAppService = webAppService;
        }

        public IEnumerable<WoxResult> GetResults(WoxQuery query)
        {
            switch (query.FirstTerm)
            {
                case "list":
                    return GetList(query.SearchTerms.Skip(1));

                case "config":
                    return GetConfig(query);

                case "add":
                    return GetAddResults(query);

                case "remove":
                    return GetRemoveResults(query);

                case "import":
                    return GetImportResults(query);

                default:
                    var commands = GetCommandHelp(query.FirstTerm);
                    if (commands.Any())
                    {
                        return commands;
                    }
                    return GetList(query.SearchTerms);
            }
        }

        private IEnumerable<WoxResult> GetConfig(WoxQuery query)
        {
            var configuration = WebAppService.GetConfiguration();
            if (query.SearchTerms.Length > 1)
            {
                var launcher = query.SearchTerms[1];
                string arguments = null;
                if (query.SearchTerms.Length > 2)
                {
                    arguments = string.Join(" ", query.SearchTerms.Skip(2).ToArray());
                }
                string argumentsCommandLine = arguments ?? "[APP_ARGUMENT_PATTERN]";
                string argumentsReal = arguments ?? configuration.WebAppArgumentPattern;

                string title = string.Format("config {0} {1}", launcher, argumentsCommandLine);
                string subTitle = string.Format("Change webapp launcher to [{0}] and argument to [{1}]", launcher, argumentsReal);
                if (!argumentsReal.Contains("{0}"))
                {
                    subTitle = string.Format("You should consider having [{0}] inside arguments. Now it contains only [{1}]", "{0}", argumentsReal);
                }
                yield return GetActionResult
                (
                    title,
                    subTitle,
                    () =>
                    {
                        WebAppService.UpdateLauncher(launcher, argumentsReal);
                    }
                );
            }
            else
            {
                yield return GetCompletionResultFinal("config [APP_PATH] [APP_ARGUMENT_PATTERN]", "Configure a new webapp launcher - Select this item to complete the current config", () => string.Format("config {0} {1}", configuration.WebAppLauncher, configuration.WebAppArgumentPattern));
            }
        }

        private IEnumerable<WoxResult> GetAddResults(WoxQuery query)
        {
            if (query.SearchTerms.Length > 1)
            {
                var url = query.SearchTerms[1];
                var keywords = string.Join(" ", query.SearchTerms.Skip(2).ToArray());
                yield return GetActionResult
                    (
                        string.Format("add {0} {1}", url, keywords),
                        string.Format("Add the url {0}", url),
                        () =>
                        {
                            WebAppService.AddWebAppItem(url, keywords);
                        }
                    );
            }
            else
            {
                yield return HelpAdd;
            }
        }

        private IEnumerable<WoxResult> GetRemoveResults(WoxQuery query)
        {
            if (query.SearchTerms.Length > 1)
            {
                var webAppItems = WebAppService
                    .Search(query.SearchTerms.Skip(1));
                string urlTyped = null;
                if (query.SearchTerms.Length == 2)
                {
                    urlTyped = query.SearchTerms[1];
                }
                var results = webAppItems
                .Select
                (
                    item =>
                        (urlTyped != null && item.Url == urlTyped)
                        ?
                        GetActionResult
                        (
                            string.Format("remove {0}", urlTyped),
                            string.Format("Remove the url {0}", urlTyped),
                            () => WebAppService.RemoveUrl(urlTyped)
                        )
                        :
                        GetCompletionResultFinal
                        (
                            string.Format("remove {0}", item.Url),
                            string.Format("Prepare to remove {0}", item.Url),
                            () => string.Format("remove {0}", item.Url)
                        )
                );
                foreach (var result in results)
                {
                    yield return result;
                }
            }
            else
            {
                yield return GetCompletionResult("remove URL|PATTERN", "Remove an existing url", () => "remove");
            }
        }

        public IEnumerable<WoxResult> GetList(IEnumerable<string> terms)
        {
            return WebAppService
                .Search(terms)
                .Select
                (
                    item => GetActionResult
                    (
                        string.Format("Start {0}", item.Url),
                        string.Format("Start the url {0} ({1})", item.Url, item.Keywords),
                        () =>
                        {
                            WebAppService.StartUrl(item.Url);
                        }
                    )
                )
                .ToList();
        }

        private IEnumerable<WoxResult> GetImportResults(WoxQuery query)
        {
            if (query.SearchTerms.Length > 1)
            {
                var filename = string.Join(" ", query.SearchTerms.Skip(1).ToArray());
                if (WebAppService.FileExists(filename))
                {
                    yield return GetActionResult
                    (
                        string.Format("import {0}", filename),
                        string.Format("Import urls from [{0}]", filename),
                        () => WebAppService.Import(filename)
                    );
                }
                else
                {
                    yield return GetCompletionResultFinal
                    (
                        string.Format("import {0}", filename),
                        string.Format("[{0}] does not exists", filename),
                        () => string.Format("import {0}", filename)
                    );
                }
            }
            else
            {
                yield return HelpImport;
            }
        }

        private WoxResult _helpList = null;
        private WoxResult _helpConfig = null;
        private WoxResult _helpAdd = null;
        private WoxResult _helpRemove = null;
        private WoxResult _helpExport = null;
        private WoxResult _helpImport = null;

        private WoxResult HelpList => _helpList ?? (_helpList = GetCompletionResult("list [PATTERN] [PATTERN] [...]", "List all url matching patterns", () => "list"));
        private WoxResult HelpConfig => _helpConfig ?? (_helpConfig = GetCompletionResult("config [APP_PATH] [APP_ARGUMENT_PATTERN]", "Configure a new webapp launcher", () => "config"));
        private WoxResult HelpAdd => _helpAdd ?? (_helpAdd = GetCompletionResult("add URL [KEYWORD] [KEYWORD] [...]", "Add a new url (or update an existing) with associated keywords", () => "add"));
        private WoxResult HelpRemove => _helpRemove ?? (_helpRemove = GetCompletionResult("remove URL", "Remove an existing url", () => "remove"));
        private WoxResult HelpExport => _helpExport ?? (_helpExport = GetActionResult("export", "Export urls to a file", WebAppService.Export));
        private WoxResult HelpImport => _helpImport ?? (_helpImport = GetCompletionResult("import FILENAME", "Import urls from FILENAME", () => "import"));

        private bool PatternMatch(string pattern, string command) => string.IsNullOrEmpty(pattern) || command.Contains(pattern);

        private IEnumerable<WoxResult> GetCommandHelp(string pattern)
        {
            if (PatternMatch(pattern, "list")) yield return HelpList;
            if (PatternMatch(pattern, "config")) yield return HelpConfig;
            if (PatternMatch(pattern, "add")) yield return HelpAdd;
            if (PatternMatch(pattern, "remove")) yield return HelpRemove;
            if (PatternMatch(pattern, "export")) yield return HelpExport;
            if (PatternMatch(pattern, "import")) yield return HelpImport;
        }

        public WoxResult GetActionResult(string title, string subTitle, Action action) => new WoxResult
        {
            Title = title,
            SubTitle = subTitle,
            Action = () =>
            {
                action();
                WoxContextService.ChangeQuery("");
            },
            ShouldClose = true,
        };

        public WoxResult GetCompletionResult(string title, string subTitle, Func<string> getNewQuery) => new WoxResult
        {
            Title = title,
            SubTitle = subTitle,
            Action = () => WoxContextService.ChangeQuery(WoxContextService.ActionKeyword + WoxContextService.Seperater + getNewQuery() + WoxContextService.Seperater),
            ShouldClose = false,
        };

        public WoxResult GetCompletionResultFinal(string title, string subTitle, Func<string> getNewQuery) => new WoxResult
        {
            Title = title,
            SubTitle = subTitle,
            Action = () => WoxContextService.ChangeQuery(WoxContextService.ActionKeyword + WoxContextService.Seperater + getNewQuery()),
            ShouldClose = false,
        };
    }
}