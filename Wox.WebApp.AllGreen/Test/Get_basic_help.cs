using AllGreen.Lib;
using Wox.WebApp.AllGreen.Fixture;
using Wox.WebApp.AllGreen.Helper;

namespace Wox.WebApp.AllGreen.Test
{
    public class Get_basic_help : TestBase<WebAppContext>
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
            .DoAction(f => f.Write_query("wap"))
            .EndUsing()

            .UsingList<Wox_results_fixture>()
            .With<Wox_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("list [PATTERN] [PATTERN] [...]", "List all url matching patterns")
            .Check("config [APP_PATH] [APP_ARGUMENT_PATTERN]", "Configure a new webapp launcher")
            .Check("add URL [KEYWORD] [KEYWORD] [...]", "Add a new url (or update an existing) with associated keywords")
            .Check("remove [URL|PATTERN]", "Remove an existing url")
            .Check("edit [URL|PATTERN] [ -> URL [KEYWORD] [KEYWORD] [...]]", "Edit an existing url")
            .Check("open URL", "Open an url as a web app without saving it")
            .Check("export", "Export urls to a file")
            .Check("import FILENAME", "Import urls from FILENAME")
            .EndUsing()

             .Using<Wox_bar_fixture>()
            .DoAction(f => f.Append__on_query(" o"))
            .EndUsing()

             .UsingList<Wox_results_fixture>()
            .With<Wox_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("config [APP_PATH] [APP_ARGUMENT_PATTERN]", "Configure a new webapp launcher")
            .Check("remove [URL|PATTERN]", "Remove an existing url")
            .Check("open URL", "Open an url as a web app without saving it")
            .Check("export", "Export urls to a file")
            .Check("import FILENAME", "Import urls from FILENAME")
            .EndUsing()

          .EndTest();
    }
}