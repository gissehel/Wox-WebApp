using AllGreen.Lib;
using Wox.WebApp.AllGreen.Fixture;
using Wox.WebApp.AllGreen.Helper;

namespace Wox.WebApp.AllGreen.Test
{
    public class Import_urls_from_file : TestBase<WebAppContext>
    {
        public override void DoTest() =>
            StartTest()

            .IsRunnable()

            .Include<Prepare_common_context>()

            .UsingSetup<Generate_file_fixture>()
            .With(f => f.Line)
            .Enter("# launcher: chrome.exe")
            .Enter("# argumentsPattern: --app=\"{0}\"")
            .Enter("https://github.com/ (dev opensource repository)")
            .Enter("https://microsoft.com/ (corporate windows)")
            .EndUsing()

            .Using<Generate_file_fixture>()
            .DoAction(f => f.Save_last_file_to(@"C:\path\on\filesystem\filename.wap.txt"))
            .EndUsing()

            .Using<Wox_bar_fixture>()

            .DoAction(f => f.Write_query("wap imp"))
            .DoCheck(f => f.The_number_of_results_is(), "1")
            .DoCheck(f => f.The_title_of_result__is(1), "import FILENAME")
            .DoCheck(f => f.The_subtitle_of_result__is(1), "Import urls from FILENAME")

            .DoAction(f => f.Select_line(1))
            .DoAccept(f => f.Wox_is_displayed())

            .DoCheck(f => f.The_current_query_is(), "wap import ")
            .DoCheck(f => f.The_number_of_results_is(), "1")
            .DoCheck(f => f.The_title_of_result__is(1), "import FILENAME")
            .DoCheck(f => f.The_subtitle_of_result__is(1), "Import urls from FILENAME")

            .DoAction(f => f.Select_line(1))
            .DoAccept(f => f.Wox_is_displayed())
            .DoCheck(f => f.The_current_query_is(), "wap import ")

            .DoAction(f => f.Write_query("wap import filename.wap.txt"))
            .DoCheck(f => f.The_number_of_results_is(), "1")
            .DoCheck(f => f.The_title_of_result__is(1), "import filename.wap.txt")
            .DoCheck(f => f.The_subtitle_of_result__is(1), "[filename.wap.txt] does not exists")

            .DoAction(f => f.Select_line(1))
            .DoAccept(f => f.Wox_is_displayed())
            .DoCheck(f => f.The_current_query_is(), "wap import filename.wap.txt")

            .DoAction(f => f.Write_query(@"wap import C:\path\on\filesystem\filename.wap"))
            .DoCheck(f => f.The_number_of_results_is(), "1")
            .DoCheck(f => f.The_title_of_result__is(1), @"import C:\path\on\filesystem\filename.wap")
            .DoCheck(f => f.The_subtitle_of_result__is(1), @"[C:\path\on\filesystem\filename.wap] does not exists")

            .DoAction(f => f.Select_line(1))
            .DoAccept(f => f.Wox_is_displayed())
            .DoCheck(f => f.The_current_query_is(), @"wap import C:\path\on\filesystem\filename.wap")

            .DoAction(f => f.Write_query(@"wap import C:\path\on\filesystem\filename.wap.txt"))
            .DoCheck(f => f.The_number_of_results_is(), "1")
            .DoCheck(f => f.The_title_of_result__is(1), @"import C:\path\on\filesystem\filename.wap.txt")
            .DoCheck(f => f.The_subtitle_of_result__is(1), @"Import urls from [C:\path\on\filesystem\filename.wap.txt]")

            .DoAction(f => f.Select_line(1))
            .DoReject(f => f.Wox_is_displayed())
            .DoAction(f => f.Display_wox())

            .DoAction(f => f.Write_query(@"wap git"))
            .DoCheck(f => f.The_number_of_results_is(), "1")
            .DoCheck(f => f.The_title_of_result__is(1), "Start https://github.com/")
            .DoCheck(f => f.The_subtitle_of_result__is(1), "Start the url https://github.com/ (dev opensource repository)")

            .EndUsing()

            .EndTest();
    }
}