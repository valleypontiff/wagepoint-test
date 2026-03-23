using NUnit.Framework;
using Wagepoint.Tests.Pages;
using Wagepoint.Tests.Enums;

namespace Wagepoint.Tests
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class PricingTests : TestBase
    {
        // A single test function to exercise the cost-per-month calculator with a variety of inputs. It's not an exhaustive list of test cases, but it gives
        // an idea of how I approach automation, and some of tests I would think of
        [TestCase("1", PayFrequency.Weekly, 46.0)]
        [TestCase("10002", PayFrequency.Weekly, 60052.0)]
        [TestCase("1", PayFrequency.BiWeekly, 46.0)]
        [TestCase("10002", PayFrequency.BiWeekly, 60052.0)]
        [TestCase("1", PayFrequency.SemiMonthly, 46.0)]
        [TestCase("10002", PayFrequency.SemiMonthly, 60052.0)]
        [TestCase("1", PayFrequency.Monthly, 24.0)]
        [TestCase("10002", PayFrequency.Monthly, 40028.0)]
        [TestCase("0", PayFrequency.Weekly, 40.0)] // it currently accepts 0; whether it should or not is hard for me to say
        [TestCase("0", PayFrequency.BiWeekly, 40.0)]
        [TestCase("0", PayFrequency.SemiMonthly, 40.0)]
        [TestCase("0", PayFrequency.Monthly, 20.0)]
        [TestCase("-1", PayFrequency.Weekly, null)] // it currently accepts negative numbers; I would argue this is a bug; I've set the expectation to "$-", but there are possibilities
        //[TestCase("-1", PayFrequency.BiWeekly, null)] // given how the above case works, there's probably little value in testing every frequency with a negative; I would do it as part of exploratory testing, but not automate it
        //[TestCase("-1", PayFrequency.SemiMonthly, null)]
        //[TestCase("-1", PayFrequency.Monthly, null)]
        [TestCase("1.99", PayFrequency.Monthly, 24.0)] // it currently rounds down or truncates; what should it do? there are arguments for what's best, but I'll just test the current behaviour
        [TestCase("1e2", PayFrequency.Monthly, 420.0)] // it currently trucates to 1; I would argue this is a bug, so I'm letting this test fail with the expectation that "1e2" should be treated as "100", but there are, of course, other ways to handle it
        //[TestCase("1,000", PayFrequency.BiWeekly, 4020.0)] // the input type is "number", so it doesn't allow commas; validation elsewhere should catch this, but I'm leaving this here and commented out for demonstration purposes
        [TestCase("18446744073709551616", PayFrequency.BiWeekly, 110680464442257309696.0)] // max ulong + 1; the calculator is probably using a float or double, but since it doesn't handle scientific notation format, entering 300+ zeros would best be done in separate tests
        //[TestCase("one", PayFrequency.Weekly, 40.0)] // type input type is "number"; validation elesewhere should catch this; leaving this here and commented out for demonstration purposes
        public async Task ShouldCalculateCost(string numberOfEmployees, PayFrequency payFrequency, double? expectedCost)
        {
            var page = new PricingPage(Page);
            await page.GotoAsync();
            await page.CalculateCostPerMonth(numberOfEmployees, payFrequency);
            if (expectedCost.HasValue)
            {
                await Expect(page.CostPerMonthText).ToHaveTextAsync($"${expectedCost:F2}"); // there is a currency format ("C"), but it adds a comma for thousands where the UI does not; personally, I think that's a weakness of the UI
            }
            else
            {
                await Expect(page.CostPerMonthText).ToHaveTextAsync($"$-");
            }
        }
    }
}
