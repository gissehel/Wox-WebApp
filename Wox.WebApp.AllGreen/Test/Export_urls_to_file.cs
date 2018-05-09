using AllGreen.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wox.WebApp.AllGreen.Helper;
using Wox.WebApp.AllGreen.Fixture;

namespace Wox.WebApp.AllGreen.Test
{
    public class Export_urls_to_file : TestBase<WebAppContext>
    {
        public override void DoTest() =>
            StartTest()

            .IsRunnable()

            .Include<Prepare_common_context>()

            .Using<Wox_bar_fixture>()
            .DoAction(f => f.Write_query("wap exp"))
            .DoCheck(f => f.The_number_of_results_is(), "1")
            .DoCheck(f => f.The_title_of_result__is(1), "export")
            .DoCheck(f => f.The_subtitle_of_result__is(1), "Export urls to a file")
            .DoAction(f => f.Select_line(1))
            .DoReject(f => f.Wox_is_displayed())
            .EndUsing()

            .UsingList<Command_line_started_fixture>()
            .With<Command_line_started_fixture.Result>(f => f.Command, f => f.Arguments)
            .Check(@".\ExportDirectory", "")
            .EndUsing()

            .Using<Last_file_generated_fixture>()
            .DoCheck(f => f.The_filename_is(), @".\ExportDirectory\Wox.WebApp-Save-UID.wap.txt")
            .EndUsing()

            .UsingList<Last_file_generated_fixture>()
            .With<Last_file_generated_fixture.Result>(f => f.Line)
            .Check("# launcher: chrome.exe")
            .Check("# argumentsPattern: --app=\"{0}\"")
            .Check("https://google.com/ google search engine")
            .Check("https://bing.com/ bing search engine")
            .Check("https://stackoverflow.com/ questions answers")
            .Check("https://netflix.com/ video")
            .EndUsing()

            .EndTest();
    }
}