using AllGreen.Lib;
using Wox.WebApp.AllGreen.Fixture;
using Wox.WebApp.AllGreen.Helper;

namespace Wox.WebApp.AllGreen.Test
{
    public class Prepare_common_context : TestBase<WebAppContext>
    {
        public override void DoTest() =>
            StartTest()

            .Using<Wox_bar_fixture>()
            .DoAction(f => f.Start_the_bar())

            .DoAction(f => f.Display_wox())
            .DoCheck(f => f.The_current_query_is(), "")
            .DoAction(f => f.Write_query("wap add https://google.com/ google search engine"))
            .DoAction(f => f.Select_line(1))
            .DoReject(f => f.Wox_is_displayed())

            .DoAction(f => f.Display_wox())
            .DoCheck(f => f.The_current_query_is(), "")
            .DoAction(f => f.Write_query("wap add https://bing.com/ bing search engine"))
            .DoAction(f => f.Select_line(1))

            .DoAction(f => f.Display_wox())
            .DoAction(f => f.Write_query("wap add https://stackoverflow.com/ questions answers"))
            .DoAction(f => f.Select_line(1))

            .DoAction(f => f.Display_wox())
            .DoAction(f => f.Write_query("wap add https://netflix.com/ video"))
            .DoAction(f => f.Select_line(1))

            .DoAction(f => f.Display_wox())
            .EndUsing()

            .EndTest();
    }
}