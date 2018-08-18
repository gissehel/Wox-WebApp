using System;
using Wox.EasyHelper.Service;
using Wox.WebApp.Core.Service;

namespace Wox.WebApp.Service
{
    public class SystemWebAppService : SystemService, ISystemWebAppService
    {
        public SystemWebAppService(string applicationName) : base(applicationName)
        {
        }

        public string GetExportPath() => ApplicationDataPath;

        public string GetUID() => string.Format("{0:yyyyMMdd-HHmmss-fff}", DateTime.Now);
    }
}