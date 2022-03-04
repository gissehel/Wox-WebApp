using AllGreen.Lib;
using Wox.WebApp.AllGreen.Fixture;
using Wox.WebApp.AllGreen.Helper;

namespace Wox.WebApp.AllGreen.Test
{
    public class Get_avanced_help : TestBase<WebAppContext>
    {
        public override void DoTest() =>
            StartTest()

            .IsRunnable()

            .Using<Wox_bar_fixture>()
            .DoAction(f => f.Start_the_bar())
            .DoAction(f => f.Display_wox())
            .DoAccept(f => f.Wox_is_displayed())
            .DoCheck(f => f.The_current_query_is(), "")
            .EndUsing()

            .UsingList<Wox_results_fixture>()
            .With<Wox_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .EndUsing()

            .Using<Wox_bar_fixture>()
            .DoAction(f => f.Write_query("wap hel"))
            .EndUsing()

            .UsingList<Wox_results_fixture>()
            .With<Wox_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("help", "Wox-WebApp version 0.0 - (Go to Wox-WebApp main website)")
            .EndUsing()

            .UsingList<Url_opened_fixture>()
            .With<Url_opened_fixture.Result>(f => f.Url)
            .EndUsing()

            .Using<Wox_bar_fixture>()
            .DoAccept(f => f.Wox_is_displayed())
            .DoAction(f => f.Select_line(1))
            .DoReject(f => f.Wox_is_displayed())
            .EndUsing()

            .UsingList<Url_opened_fixture>()
            .With<Url_opened_fixture.Result>(f => f.Url)
            .Check("https://github.com/gissehel/Wox-WebApp")
            .EndUsing()

            .EndTest();
    }
}