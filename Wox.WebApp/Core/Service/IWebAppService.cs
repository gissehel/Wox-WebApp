using System.Collections.Generic;
using Wox.WebApp.DomainModel;

namespace Wox.WebApp.Core.Service
{
    public interface IWebAppService
    {
        void Init();

        IEnumerable<WebAppItem> Search(IEnumerable<string> terms);

        void AddWebAppItem(string url, string keywords);

        void UpdateLauncher(string launcher, string argumentPattern);

        void StartUrl(string url);

        WebAppConfiguration GetConfiguration();

        void RemoveUrl(string url);
    }
}