using System.Collections.Generic;
using Wox.WebApp.DomainModel;

namespace Wox.WebApp.Core.Service
{
    public interface IWoxResultFinder
    {
        IEnumerable<WoxResult> GetResults(WoxQuery query);
    }
}