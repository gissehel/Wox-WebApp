using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Wox.EasyHelper.Core.Service;
using Wox.WebApp.Core.Service;

namespace Wox.WebApp.Service
{
    public class ApplicationInformationService : IApplicationInformationService
    {
        private ISystemService SystemService { get; set; }

        public ApplicationInformationService(ISystemService systemService)
        {
            SystemService = systemService;
        }

        public string ApplicationName => SystemService.ApplicationName;

        public string Version => FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion;

        public string HomepageUrl => "https://github.com/gissehel/Wox-WebApp";
    }
}
