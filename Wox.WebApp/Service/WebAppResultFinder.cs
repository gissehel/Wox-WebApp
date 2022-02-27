using System.Collections.Generic;
using System.Linq;
using Wox.EasyHelper;
using Wox.EasyHelper.Core.Service;
using Wox.WebApp.Core.Service;

namespace Wox.WebApp.Service
{
    public class WebAppResultFinder : WoxResultFinder
    {
        private IWebAppService WebAppService { get; set; }

        public WebAppResultFinder(IWoxContextService woxContextService, IWebAppService webAppService) : base(woxContextService)
        {
            WebAppService = webAppService;
        }

        public override void Init()
        {
            WebAppService.Init();

            AddCommand("list", "list [PATTERN] [PATTERN] [...]", "List all url matching patterns", GetListResults);
            AddCommand("config", "config [APP_PATH] [APP_ARGUMENT_PATTERN]", "Configure a new webapp launcher", GetConfigResults);
            AddCommand("add", "add URL [KEYWORD] [KEYWORD] [...]", "Add a new url (or update an existing) with associated keywords", GetAddResults);
            AddCommand("remove", "remove [URL|PATTERN]", "Remove an existing url", GetRemoveResults);
            AddCommand("edit", "edit [URL|PATTERN] [ -> URL [KEYWORD] [KEYWORD] [...]]", "Edit an existing url", GetEditResults);
            AddCommand("open", "open URL", "Open an url as a web app without saving it", GetOpenResults);
            AddCommand("export", "export", "Export urls to a file", ExportCommandAction);
            AddCommand("import", "import FILENAME", "Import urls from FILENAME", GetImportResults);

            AddDefaultCommand(GetListResults);
        }

        private IEnumerable<WoxResult> GetEditResults(WoxQuery query, int position)
        {
            if (query.SearchTerms.Contains("->"))
            {
                var url = query.GetTermOrEmpty(1);
                var oper = query.GetTermOrEmpty(2);
                if (oper == "->")
                {
                    var newUrl = query.GetTermOrEmpty(3);
                    var newKeywords = query.GetAllSearchTermsStarting(4);
                    var webAppItem = WebAppService.GetUrlInfo(url);
                    var keywords = webAppItem.Keywords;

                    if ((keywords == newKeywords) && (url == newUrl))
                    {
                        return new List<WoxResult> {
                        GetCompletionResultFinal(
                            string.Format("Edit {0}",url),
                            string.Format("Edit the url {0} ({1})", url, keywords),
                            ()=>string.Format("edit {0} -> {0} {1}", url, keywords)
                        )
                    };
                    }
                    else
                    {
                        return new List<WoxResult> {
                        GetActionResult(
                            string.Format("Edit {0}",url),
                            string.Format("Edit the url {0} ({1}) -> {2} ({3})", url, keywords, newUrl, newKeywords),
                            () =>
                            {
                                WebAppService.EditWebAppItem(url, newUrl, newKeywords);
                            }
                        )
                    };
                    }
                }
                else
                {
                    return new List<WoxResult> {
                        GetCompletionResult(
                            "edit [URL|PATTERN] [ -> URL [KEYWORD] [KEYWORD] [...]]",
                            "Edit an existing url",
                            ()=>"edit "
                        )
                    };
                }

                throw new System.NotImplementedException();
            }
            else
            {
                var terms = query.GetSearchTermsStarting(position);
                return WebAppService
                    .Search(terms)
                    .Select
                    (
                        item => GetCompletionResult
                        (
                            string.Format("Edit {0}", item.Url),
                            string.Format("Edit the url {0} ({1})", item.Url, item.Keywords),
                            () => string.Format("edit {0} -> {0} {1}", item.Url, item.Keywords)
                        )
                    );
            }
        }

        private IEnumerable<WoxResult> GetListResults(WoxQuery query, int position)
        {
            var terms = query.GetSearchTermsStarting(position);
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
                );
        }

        private IEnumerable<WoxResult> GetConfigResults(WoxQuery query, int position)
        {
            var configuration = WebAppService.GetConfiguration();
            if (query.SearchTerms.Length > position)
            {
                var launcher = query.SearchTerms[position];
                string arguments = query.GetAllSearchTermsStarting(position + 1);
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
                var emptyResult = GetEmptyCommandResult("config", CommandInfos);
                yield return GetCompletionResultFinal
                    (
                        emptyResult.Title,
                        "{0} - Select this item to complete the current config".FormatWith(emptyResult.SubTitle),
                        () => string.Format("config {0} {1}", configuration.WebAppLauncher, configuration.WebAppArgumentPattern)
                    );
            }
        }

        private IEnumerable<WoxResult> GetAddResults(WoxQuery query, int position)
        {
            var url = query.GetTermOrEmpty(position);
            if (!string.IsNullOrEmpty(url))
            {
                var keywords = query.GetAllSearchTermsStarting(position + 1);
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
                yield return GetEmptyCommandResult("add", CommandInfos);
            }
        }

        private IEnumerable<WoxResult> GetRemoveResults(WoxQuery query, int position)
        {
            var webAppItems = WebAppService
                .Search(query.GetSearchTermsStarting(position));
            string urlTyped = null;
            if (query.SearchTerms.Length == position + 1)
            {
                urlTyped = query.SearchTerms[position];
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
            if (results.Count() > 0)
            {
                foreach (var result in results)
                {
                    yield return result;
                }
            }
            else
            {
                yield return GetEmptyCommandResult("remove", CommandInfos);
            }
        }

        private IEnumerable<WoxResult> GetOpenResults(WoxQuery query, int position)
        {
            if (query.SearchTerms.Length == position + 1)
            {
                var url = query.SearchTerms[position];
                yield return GetActionResult
                (
                    "open {0}".FormatWith(url),
                    "Open the web app page [{0}] without saving it",
                    () =>
                    {
                        WebAppService.StartUrl(url);
                    }
                );
            }
            else
            {
                yield return GetEmptyCommandResult("open", CommandInfos);
            }
        }

        private void ExportCommandAction() => WebAppService.Export();

        private IEnumerable<WoxResult> GetImportResults(WoxQuery query, int position)
        {
            var filename = query.GetAllSearchTermsStarting(position);
            if (!string.IsNullOrEmpty(filename))
            {
                if (WebAppService.FileExists(filename))
                {
                    yield return GetActionResult
                    (
                        "import {0}".FormatWith(filename),
                        "Import urls from [{0}]".FormatWith(filename),
                        () => WebAppService.Import(filename)
                    );
                }
                else
                {
                    yield return GetCompletionResultFinal
                    (
                        "import {0}".FormatWith(filename),
                        "[{0}] does not exists".FormatWith(filename),
                        () => "import {0}".FormatWith(filename)
                    );
                }
            }
            else
            {
                yield return GetEmptyCommandResult("import", CommandInfos);
            }
        }

        public override void Dispose()
        {
            WebAppService.Dispose();
        }
    }
}