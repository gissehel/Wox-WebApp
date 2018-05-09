using System.Collections.Generic;
using Wox.WebApp.DomainModel;

namespace Wox.WebApp.Core.Service
{
    public interface IWebAppItemRepository
    {
        void Init();

        IEnumerable<WebAppItem> SearchItems(IEnumerable<string> terms);

        void AddItem(WebAppItem item);

        void RemoveItem(string url);
    }
}