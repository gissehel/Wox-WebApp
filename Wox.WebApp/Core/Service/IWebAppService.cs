using System;
using System.Collections.Generic;
using Wox.WebApp.DomainModel;

namespace Wox.WebApp.Core.Service
{
    public interface IWebAppService : IDisposable
    {
        void Init();

        IEnumerable<WebAppItem> Search(IEnumerable<string> terms);

        void AddWebAppItem(string url, string keywords);

        void UpdateLauncher(string launcher, string argumentPattern);

        void StartUrl(string url);

        WebAppConfiguration GetConfiguration();

        void RemoveUrl(string url);

        void Export();

        bool FileExists(string path);

        void Import(string path);
    }
}