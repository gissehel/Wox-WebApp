using AllGreen.Lib;
using Wox.WebApp.AllGreen.Fixture;
using Wox.WebApp.AllGreen.Helper;

namespace Wox.WebApp.AllGreen.Test
{
    public class Search_urls : TestBase<WebAppContext>
    {
        public override void DoTest() =>
            StartTest()
            .IsRunnable()

            .Include<Prepare_common_context>()

            .Using<Wox_bar_fixture>()
            .DoAction(f => f.Write_query("wap list"))
            .EndUsing()

            .UsingList<Wox_results_fixture>()
            .With<Wox_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("Start https://google.com/", "Start the url https://google.com/ (google search engine)")
            .Check("Start https://bing.com/", "Start the url https://bing.com/ (bing search engine)")
            .Check("Start https://stackoverflow.com/", "Start the url https://stackoverflow.com/ (questions answers)")
            .Check("Start https://netflix.com/", "Start the url https://netflix.com/ (video)")
            .EndUsing()

            .Using<Wox_bar_fixture>()
            .DoAction(f => f.Write_query("wap list gin"))
            .EndUsing()

            .UsingList<Wox_results_fixture>()
            .With<Wox_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("Start https://google.com/", "Start the url https://google.com/ (google search engine)")
            .Check("Start https://bing.com/", "Start the url https://bing.com/ (bing search engine)")
            .EndUsing()

            .Using<Wox_bar_fixture>()
            .DoAction(f => f.Write_query("wap gin"))
            .EndUsing()

            .UsingList<Wox_results_fixture>()
            .With<Wox_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("Start https://google.com/", "Start the url https://google.com/ (google search engine)")
            .Check("Start https://bing.com/", "Start the url https://bing.com/ (bing search engine)")
            .EndUsing()

            .Using<Wox_bar_fixture>()
            .DoAction(f => f.Write_query("wap video"))
            .EndUsing()

            .UsingList<Wox_results_fixture>()
            .With<Wox_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("Start https://netflix.com/", "Start the url https://netflix.com/ (video)")
            .EndUsing()

            .UsingList<Command_line_started_fixture>()
            .With<Command_line_started_fixture.Result>(f => f.Command, f => f.Arguments)
            .EndUsing()

            .Using<Wox_bar_fixture>()
            .DoAction(f => f.Select_line(1))
            .DoReject(f => f.Wox_is_displayed())
            .EndUsing()

            .UsingList<Command_line_started_fixture>()
            .With<Command_line_started_fixture.Result>(f => f.Command, f => f.Arguments)
            .Check("chrome.exe", "--app=\"https://netflix.com/\"")
            .EndUsing()

            .EndTest();
    }
}