using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wox.WebApp.Core.Service;

namespace Wox.WebApp.Mock.Service
{
    public class ApplicationInformationServiceMock : IApplicationInformationService
    {
        public string ApplicationName { get; set; }

        public string Version { get; set; }

        public string HomepageUrl { get; set; }
    }
}
