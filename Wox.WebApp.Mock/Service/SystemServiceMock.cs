using Wox.EasyHelper.Test.Mock.Service;
using Wox.WebApp.Core.Service;

namespace Wox.WebApp.Mock.Service
{
    public class SystemWebAppServiceMock : SystemServiceMock, ISystemWebAppService
    {
        public string DatabaseName => ApplicationName;

        public string GetExportPath() => @".\ExportDirectory";

        public string GetUID() => "UID";
    }
}