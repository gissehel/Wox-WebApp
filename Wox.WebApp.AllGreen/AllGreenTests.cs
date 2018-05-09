using AllGreen.Lib;
using AllGreen.Lib.Core.Engine.Service;
using AllGreen.Lib.DomainModel.Script;
using AllGreen.Lib.Engine.Service;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Wox.WebApp.AllGreen.Helper;

namespace Wox.WebApp.AllGreen

{
    [TestFixture]
    public class AllGreenTests
    {
        private ITestRunnerService _testRunnerService = null;
        private ITestRunnerService TestRunnerService => _testRunnerService ?? (_testRunnerService = new TestRunnerService());

        private Dictionary<string, TestScript<WebAppContext>> GetTestScripts()
        {
            var testScripts = new Dictionary<string, TestScript<WebAppContext>>();
            var allRunnableTestScripts = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.IsSubclassOf(typeof(TestBase<WebAppContext>)))
                .Select(t => (Activator.CreateInstance(t) as TestBase<WebAppContext>).GetTestScript())
                .Where(s => s != null)
                .Where(s => s.IsRunnable)
                ;

            foreach (var testScript in allRunnableTestScripts)
            {
                testScripts[testScript.Name] = testScript;
            }
            return testScripts;
        }

        private Dictionary<string, TestScript<WebAppContext>> _testScripts = null;
        private Dictionary<string, TestScript<WebAppContext>> TestScripts => _testScripts ?? (_testScripts = GetTestScripts());

        private IEnumerable<string> GetTestScriptNames() => TestScripts.Keys.OrderBy(name => name);

        [TestCaseSource(nameof(GetTestScriptNames))]
        public void Run(string name)
        {
            if (TestScripts.ContainsKey(name))
            {
                RunTest(TestScripts[name]);
            }
            else
            {
                Assert.Fail(string.Format("Don't know [{0}] as a test name !", name));
            }
        }

        private void RunTest(TestScript<WebAppContext> testScript)
        {
            var result = TestRunnerService.RunTest(testScript);

            Assert.IsNotNull(result, "The test returned a null result. Is the test runnable ?");
            Assert.IsTrue(result.Success, result.PipedName);
        }
    }
}