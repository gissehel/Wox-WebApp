using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wox.WebApp.Core.Service
{
    public interface IApplicationInformationService
    {
        string ApplicationName { get; }

        string Version { get; }

        string HomepageUrl { get; }
    }
}
