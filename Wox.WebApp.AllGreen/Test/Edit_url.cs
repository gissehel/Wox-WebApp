using AllGreen.Lib;
using Wox.WebApp.AllGreen.Fixture;
using Wox.WebApp.AllGreen.Helper;

namespace Wox.WebApp.AllGreen.Test
{
    internal class Edit_url : TestBase<WebAppContext>
    {
        public override void DoTest() =>
            StartTest()
            .IsRunnable()
            .Include<Prepare_common_context>()

            .Using<Wox_bar_fixture>()
            .DoAction(f => f.Write_query("wap edi"))
            .DoCheck(f => f.The_number_of_results_is(), "1")
            .DoCheck(f => f.The_title_of_result__is(1), "edit [URL|PATTERN] [ -> URL [KEYWORD] [KEYWORD] [...]]")
            .DoCheck(f => f.The_subtitle_of_result__is(1), "Edit an existing url")
            .DoAction(f => f.Select_line(1))
            .DoAccept(f => f.Wox_is_displayed())

            .DoCheck(f => f.The_current_query_is(), "wap edit ")
            .DoCheck(f => f.The_number_of_results_is(), "4")
            .EndUsing()

            .UsingList<Wox_results_fixture>()
            .With<Wox_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("Edit https://google.com/", "Edit the url https://google.com/ (google search engine)")
            .Check("Edit https://bing.com/", "Edit the url https://bing.com/ (bing search engine)")
            .Check("Edit https://stackoverflow.com/", "Edit the url https://stackoverflow.com/ (questions answers)")
            .Check("Edit https://netflix.com/", "Edit the url https://netflix.com/ (video)")
            .EndUsing()

            .Using<Wox_bar_fixture>()
            .DoAction(f => f.Append__on_query("bin"))
            .DoCheck(f => f.The_number_of_results_is(), "1")
            .DoCheck(f => f.The_title_of_result__is(1), "Edit https://bing.com/")
            .DoCheck(f => f.The_subtitle_of_result__is(1), "Edit the url https://bing.com/ (bing search engine)")
            .DoAction(f => f.Select_line(1))
            .DoAccept(f => f.Wox_is_displayed())

            .DoCheck(f => f.The_current_query_is(), "wap edit https://bing.com/ -> https://bing.com/ bing search engine ")
            .DoCheck(f => f.The_number_of_results_is(), "1")
            .DoCheck(f => f.The_title_of_result__is(1), "Edit https://bing.com/")
            .DoCheck(f => f.The_subtitle_of_result__is(1), "Edit the url https://bing.com/ (bing search engine)")

            .DoAction(f => f.Write_query("wap edit https://bing.com/ -> https://bing.com/ bing microsoft search engine"))
            .DoCheck(f => f.The_number_of_results_is(), "1")
            .DoCheck(f => f.The_title_of_result__is(1), "Edit https://bing.com/")
            .DoCheck(f => f.The_subtitle_of_result__is(1), "Edit the url https://bing.com/ (bing search engine) -> https://bing.com/ (bing microsoft search engine)")

            .DoAction(f => f.Select_line(1))
            .DoReject(f => f.Wox_is_displayed())
            .DoAction(f => f.Display_wox())

            .DoAction(f => f.Write_query("wap list"))
            .EndUsing()


            .UsingList<Wox_results_fixture>()
            .With<Wox_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("Start https://google.com/", "Start the url https://google.com/ (google search engine)")
            .Check("Start https://bing.com/", "Start the url https://bing.com/ (bing microsoft search engine)")
            .Check("Start https://stackoverflow.com/", "Start the url https://stackoverflow.com/ (questions answers)")
            .Check("Start https://netflix.com/", "Start the url https://netflix.com/ (video)")
            .EndUsing()

            .EndTest();
    }
}
