using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using Wagepoint.Tests.Config;

namespace Wagepoint.Tests
{
    [TestFixture]
    public class TestBase : PageTest
    {
        [OneTimeSetUp]
        public void SetUpFixture()
        {
            Config.Config.LoadConfig(TestContext.Parameters);
        }

        [SetUp]
        public async Task SetUpTest()
        {
            await Context.Tracing.StartAsync(new()
            {
                Title = TestContext.CurrentContext.Test.FullName,
                Screenshots = true,
                Snapshots = true,
                Sources = true
            });
        }

        [TearDown]
        public async Task TearDownTest()
        {
            if (Config.Config.SaveTraces == SaveTraces.Always ||
                (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed && Config.Config.SaveTraces == SaveTraces.OnFailure))
            {
                await Context.Tracing.StopAsync(new()
                {
                    Path = Path.Combine(TestContext.CurrentContext.WorkDirectory, "traces", $"{SanitizeFileName(TestContext.CurrentContext.Test.FullName)}.zip")
                });
            }
            else
            {
                await Context.Tracing.StopAsync();
            }
        }

        private static string SanitizeFileName(string name)
        {
            foreach (var c in Path.GetInvalidFileNameChars())
            {
                name = name.Replace(c, '_');
            }

            return name;
        }
    }
}
