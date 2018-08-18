using FluentDataAccess.Core.Service;
using System.Collections.Generic;
using System.Text;
using Wox.WebApp.Core.Service;
using Wox.WebApp.DomainModel;

namespace Wox.WebApp.Service
{
    public class WebAppItemRepository : IWebAppItemRepository
    {
        private IDataAccessService DataAccessService { get; set; }

        public WebAppItemRepository(IDataAccessService dataAccessService)
        {
            DataAccessService = dataAccessService;
        }

        public void Init()
        {
            DataAccessService
                .GetQuery("create table if not exists webapp_item (id integer primary key, url text, keywords text, search text)")
                .Execute();
            DataAccessService
                .GetQuery("create unique index if not exists webapp_item_url on webapp_item (url)")
                .Execute();
        }

        public void AddItem(WebAppItem item)
        {
            DataAccessService
                .GetQuery("insert or replace into webapp_item (url, keywords, search) values (@url, @keywords, @search)")
                .WithParameter("url", item.Url)
                .WithParameter("keywords", item.Keywords)
                .WithParameter("search", string.Format("{0} {1}", item.Url, item.Keywords).ToLower())
                .Execute();
        }

        public void RemoveItem(string url)
        {
            DataAccessService
                .GetQuery("delete from webapp_item where url=@url")
                .WithParameter("url", url)
                .Execute();
        }

        public IEnumerable<WebAppItem> SearchItems(IEnumerable<string> terms)
        {
            var builder = new StringBuilder("select id, url, keywords from webapp_item ");
            int index = 0;
            foreach (var term in terms)
            {
                if (index == 0)
                {
                    builder.Append("where ");
                }
                else
                {
                    builder.Append("and ");
                }
                builder.Append("search like @param");
                builder.Append(index.ToString());
                builder.Append(" ");
                index++;
            }
            builder.Append("order by id");
            var dataAccessQuery = DataAccessService.GetQuery(builder.ToString());
            index = 0;
            foreach (var term in terms)
            {
                var parameterName = string.Format("param{0}", index);
                var parameterValue = string.Format("%{0}%", term.ToLower());
                dataAccessQuery = dataAccessQuery.WithParameter(parameterName, parameterValue);
                index++;
            }
            return dataAccessQuery
                .Returning<WebAppItem>()
                .Reading("id", (WebAppItem item, long value) => item.Id = value)
                .Reading("url", (WebAppItem item, string value) => item.Url = value)
                .Reading("keywords", (WebAppItem item, string value) => item.Keywords = value)
                .Execute()
                ;
        }
    }
}