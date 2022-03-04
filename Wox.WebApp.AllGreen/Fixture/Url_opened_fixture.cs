using AllGreen.Lib;
using System.Collections.Generic;
using Wox.WebApp.AllGreen.Helper;

namespace Wox.WebApp.AllGreen.Fixture
{
    public class Url_opened_fixture : FixtureBase<WebAppContext>
    {
        public override IEnumerable<object> OnQuery()
        {
            foreach (var url in Context.ApplicationStarter.SystemService.UrlOpened)
            {
                yield return new Result { Url = url };
            }
        }

        public class Result
        {
            public string Url { get; set; }
        }
    }
}