using FluentDataAccess.Core.Service;
using Wox.EasyHelper.Core.Service;

namespace Wox.WebApp.Core.Service
{
    public interface ISystemWebAppService : ISystemService, IDataAccessConfigurationService
    {
        string GetExportPath();

        string GetUID();
    }
}