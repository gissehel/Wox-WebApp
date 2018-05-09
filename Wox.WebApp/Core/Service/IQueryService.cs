using Wox.Plugin;
using Wox.WebApp.DomainModel;

namespace Wox.WebApp.Core.Service
{
    public interface IQueryService
    {
        WoxQuery GetWoxQuery(Query pluginQuery);
    }
}