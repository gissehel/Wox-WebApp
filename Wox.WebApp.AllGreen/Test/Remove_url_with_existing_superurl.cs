﻿using AllGreen.Lib;
using Wox.WebApp.AllGreen.Fixture;
using Wox.WebApp.AllGreen.Helper;

namespace Wox.WebApp.AllGreen.Test
{
    public class Remove_url_with_existing_superurl : TestBase<WebAppContext>
    {
        public override void DoTest() =>
            StartTest()

            .IsRunnable()

            .Include<Prepare_common_context>()

            .Comment("We're adding https://bing.com while https://bing.com/ already exists in our context")

            .Using<Wox_bar_fixture>()
            .DoAction(f => f.Write_query("wap add https://bing.com ms search"))
            .DoCheck(f => f.The_number_of_results_is(), "1")
            .DoCheck(f => f.The_title_of_result__is(1), "add https://bing.com ms search [default]")
            .DoCheck(f => f.The_subtitle_of_result__is(1), "Add the url https://bing.com")
            .DoAction(f => f.Select_line(1))
            .DoReject(f => f.Wox_is_displayed())
            .DoAction(f => f.Display_wox())

            .Comment("We're now searching for bing...")

            .DoAction(f => f.Write_query("wap bing"))
            .DoCheck(f => f.The_number_of_results_is(), "2")

            .Comment("Woops 2 results...")

            .EndUsing()

            .UsingList<Wox_results_fixture>()
            .With<Wox_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("Start https://bing.com/", "Start the url https://bing.com/ (bing search engine) [default]")
            .Check("Start https://bing.com", "Start the url https://bing.com (ms search) [default]")
            .EndUsing()

            .Comment("We're goind to remove the second one")

            .Using<Wox_bar_fixture>()
            .DoAction(f => f.Write_query("wap remove bing"))
            .DoCheck(f => f.The_number_of_results_is(), "2")
            .DoCheck(f => f.The_title_of_result__is(2), "remove https://bing.com")
            .DoCheck(f => f.The_subtitle_of_result__is(2), "Prepare to remove https://bing.com")
            .DoAction(f => f.Select_line(2))
            .DoCheck(f => f.The_current_query_is(), "wap remove https://bing.com")

            .EndUsing()

            .UsingList<Wox_results_fixture>()
            .With<Wox_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("remove https://bing.com/", "Prepare to remove https://bing.com/")
            .Check("remove https://bing.com", "Remove the url https://bing.com")
            .EndUsing()

            .Using<Wox_bar_fixture>()
            .DoAction(f => f.Select_line(2))
            .DoReject(f => f.Wox_is_displayed())
            .DoAction(f => f.Display_wox())
            .DoAction(f => f.Write_query("wap bing"))

            .DoCheck(f => f.The_number_of_results_is(), "1")

            .Comment("Now there is only 1 result for bing")

            .DoCheck(f => f.The_title_of_result__is(1), "Start https://bing.com/")
            .DoCheck(f => f.The_subtitle_of_result__is(1), "Start the url https://bing.com/ (bing search engine) [default]")

            .EndUsing()

            .EndTest();
    }
}