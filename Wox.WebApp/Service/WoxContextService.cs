using Wox.Plugin;
using Wox.WebApp.Core.Service;

namespace Wox.WebApp.Service
{
    public class WoxContextService : IWoxContextService
    {
        private PluginInitContext Context { get; set; }

        public WoxContextService(PluginInitContext context)
        {
            Context = context;
        }

        public string ActionKeyword => Context.CurrentPluginMetadata.ActionKeyword;

        public string Seperater => Query.Seperater;

        public void ChangeQuery(string query) => Context.API.ChangeQuery(query);
    }
}