using AllGreen.Lib;
using Wox.WebApp.AllGreen.Fixture;
using Wox.WebApp.AllGreen.Helper;

namespace Wox.WebApp.AllGreen.Test
{
    public class Change_configuration : TestBase<WebAppContext>
    {
        public override void DoTest() =>
            StartTest()

            .IsRunnable()

            .Include<Prepare_common_context>()

            .Comment("We're going to reconfigure the plugin with a new web app executable")

            .Using<Wox_bar_fixture>()
            .DoAction(f => f.Write_query("wap con"))
            .EndUsing()

            .UsingList<Wox_results_fixture>()
            .With<Wox_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("config [APP_PATH] [APP_ARGUMENT_PATTERN]", "Configure a new webapp launcher")
            .EndUsing()

            .Comment("When we select the only result for query 'wap con', we expect the completion to 'wap config '.")

            .Using<Wox_bar_fixture>()
            .DoAction(f => f.Select_line(1))
            .DoAccept(f => f.Wox_is_displayed())
            .DoCheck(f => f.The_current_query_is(), "wap config ")
            .DoCheck(f => f.The_number_of_results_is(), "1")
            .DoCheck(f => f.The_title_of_result__is(1), "config [APP_PATH] [APP_ARGUMENT_PATTERN]")
            .DoCheck(f => f.The_subtitle_of_result__is(1), "Configure a new webapp launcher - Select this item to complete the current config")
            .DoAction(f => f.Select_line(1))

            .Comment("Now the query is 'wap config ', we expect that what look like the same result will make completion to the actual configuration.")

            .DoAccept(f => f.Wox_is_displayed())
            .DoCheck(f => f.The_current_query_is(), "wap config chrome.exe --app=\"{0}\"")
            .DoCheck(f => f.The_number_of_results_is(), "1")
            .DoCheck(f => f.The_title_of_result__is(1), "config chrome.exe --app=\"{0}\"")
            .DoCheck(f => f.The_subtitle_of_result__is(1), "Change webapp launcher to [chrome.exe] and argument to [--app=\"{0}\"]")

            .Comment("We're now going to change only the launcher and not the argument pattern.")

            .DoAccept(f => f.Wox_is_displayed())
            .DoAction(f => f.Write_query("wap config mywepapplauncher.exe"))
            .DoCheck(f => f.The_number_of_results_is(), "1")
            .DoCheck(f => f.The_title_of_result__is(1), "config mywepapplauncher.exe [APP_ARGUMENT_PATTERN]")
            .DoCheck(f => f.The_subtitle_of_result__is(1), "Change webapp launcher to [mywepapplauncher.exe] and argument to [--app=\"{0}\"]")

            .DoAction(f => f.Select_line(1))
            .DoReject(f => f.Wox_is_displayed())

            .Comment("We expect now the configuration to be changed. We're going to verify that.")

            .DoAction(f => f.Display_wox())
            .DoAction(f => f.Write_query("wap video"))
            .DoCheck(f => f.The_number_of_results_is(), "1")
            .DoCheck(f => f.The_title_of_result__is(1), "Start https://netflix.com/")
            .DoCheck(f => f.The_subtitle_of_result__is(1), "Start the url https://netflix.com/ (video)")

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
            .Check("mywepapplauncher.exe", "--app=\"https://netflix.com/\"")
            .EndUsing()

            .Using<Wox_bar_fixture>()
            .DoAction(f => f.Display_wox())
            .DoAction(f => f.Write_query("wap config mywepapplauncher.exe "))
            .DoCheck(f => f.The_number_of_results_is(), "1")
            .DoCheck(f => f.The_title_of_result__is(1), "config mywepapplauncher.exe [APP_ARGUMENT_PATTERN]")
            .DoCheck(f => f.The_subtitle_of_result__is(1), "Change webapp launcher to [mywepapplauncher.exe] and argument to [--app=\"{0}\"]")

            .DoAction(f => f.Append__on_query("x"))
            .DoCheck(f => f.The_number_of_results_is(), "1")
            .DoCheck(f => f.The_title_of_result__is(1), "config mywepapplauncher.exe x")
            .DoCheck(f => f.The_subtitle_of_result__is(1), "You should consider having [{0}] inside arguments. Now it contains only [x]")

            .DoAction(f => f.Write_query("wap config mywepapplauncher.exe {0}"))
            .DoCheck(f => f.The_number_of_results_is(), "1")
            .DoCheck(f => f.The_title_of_result__is(1), "config mywepapplauncher.exe {0}")
            .DoCheck(f => f.The_subtitle_of_result__is(1), "Change webapp launcher to [mywepapplauncher.exe] and argument to [{0}]")

            .DoAction(f => f.Select_line(1))
            .DoReject(f => f.Wox_is_displayed())

            .Comment("We expect now the configuration to be changed. We're going to verify that.")

            .DoAction(f => f.Display_wox())
            .DoAction(f => f.Write_query("wap video"))
            .DoCheck(f => f.The_number_of_results_is(), "1")
            .DoCheck(f => f.The_title_of_result__is(1), "Start https://netflix.com/")
            .DoCheck(f => f.The_subtitle_of_result__is(1), "Start the url https://netflix.com/ (video)")

            .EndUsing()

            .UsingList<Command_line_started_fixture>()
            .With<Command_line_started_fixture.Result>(f => f.Command, f => f.Arguments)
            .Check("mywepapplauncher.exe", "--app=\"https://netflix.com/\"")
            .EndUsing()

            .Using<Wox_bar_fixture>()
            .DoAction(f => f.Select_line(1))
            .DoReject(f => f.Wox_is_displayed())
            .EndUsing()

            .UsingList<Command_line_started_fixture>()
            .With<Command_line_started_fixture.Result>(f => f.Command, f => f.Arguments)
            .Check("mywepapplauncher.exe", "--app=\"https://netflix.com/\"")
            .Check("mywepapplauncher.exe", "https://netflix.com/")
            .EndUsing()

            .Using<Wox_bar_fixture>()
            .DoAction(f => f.Display_wox())

            .DoAction(f => f.Write_query("wap config mywepapplauncher.exe --app=\"{0}\" --maximized"))
            .DoCheck(f => f.The_number_of_results_is(), "1")
            .DoCheck(f => f.The_title_of_result__is(1), "config mywepapplauncher.exe --app=\"{0}\" --maximized")
            .DoCheck(f => f.The_subtitle_of_result__is(1), "Change webapp launcher to [mywepapplauncher.exe] and argument to [--app=\"{0}\" --maximized]")

            .DoAction(f => f.Select_line(1))
            .DoReject(f => f.Wox_is_displayed())

            .Comment("We expect now the configuration to be changed. We're going to verify that.")

            .DoAction(f => f.Display_wox())
            .DoAction(f => f.Write_query("wap video"))
            .DoCheck(f => f.The_number_of_results_is(), "1")
            .DoCheck(f => f.The_title_of_result__is(1), "Start https://netflix.com/")
            .DoCheck(f => f.The_subtitle_of_result__is(1), "Start the url https://netflix.com/ (video)")
            .DoAction(f => f.Select_line(1))
            .DoReject(f => f.Wox_is_displayed())

            .EndUsing()

            .UsingList<Command_line_started_fixture>()
            .With<Command_line_started_fixture.Result>(f => f.Command, f => f.Arguments)
            .Check("mywepapplauncher.exe", "--app=\"https://netflix.com/\"")
            .Check("mywepapplauncher.exe", "https://netflix.com/")
            .Check("mywepapplauncher.exe", "--app=\"https://netflix.com/\" --maximized")
            .EndUsing()

            .EndTest();
    }
}