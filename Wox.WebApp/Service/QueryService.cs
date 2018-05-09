using Wox.Plugin;
using Wox.WebApp.Core.Service;
using Wox.WebApp.DomainModel;

namespace Wox.WebApp.Service
{
    public class QueryService : IQueryService
    {
        public WoxQuery GetWoxQuery(Query pluginQuery)
        {
            var searchTerms = pluginQuery.Search.Split(' ');
            return new WoxQuery
            {
                InternalQuery = pluginQuery,
                RawQuery = pluginQuery.RawQuery,
                Search = pluginQuery.Search,
                SearchTerms = searchTerms,
                Command = pluginQuery.RawQuery.Split(' ')[0],
            };
        }
    }
}