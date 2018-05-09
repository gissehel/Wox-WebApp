using AllGreen.Lib;
using System.Collections.Generic;
using System.Linq;
using Wox.WebApp.AllGreen.Helper;

namespace Wox.WebApp.AllGreen.Fixture
{
    public class Command_line_started_fixture : FixtureBase<WebAppContext>
    {
        public class Result
        {
            public string Command { get; set; }
            public string Arguments { get; set; }
        }

        public override IEnumerable<object> OnQuery()
        {
            var result = Context
                .ApplicationStarter
                .SystemService
                .CommandLineStarted
                .Select(pair => new Result
                {
                    Command = pair.Key,
                    Arguments = pair.Value
                }).ToList();
            return result.Cast<object>();
        }
    }
}