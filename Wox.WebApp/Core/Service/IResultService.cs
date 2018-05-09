using System.Collections.Generic;
using Wox.Plugin;
using Wox.WebApp.DomainModel;

namespace Wox.WebApp.Core.Service
{
    public interface IResultService
    {
        List<Result> MapResults(IEnumerable<WoxResult> results);
    }
}