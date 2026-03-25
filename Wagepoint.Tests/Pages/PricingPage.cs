using Microsoft.Playwright;
using Wagepoint.Tests.Enums;

namespace Wagepoint.Tests.Pages
{
    internal class PricingPage
    {
        private readonly IPage _page;
        private readonly ILocator _popUpCloseButton;
        private readonly ILocator _numberOfEmployeesInput;
        private readonly ILocator _paymentFrequencySelect;
        private readonly ILocator _calculateButton;
        private readonly ILocator _costPerMonthText;

        public ILocator CostPerMonthText => _costPerMonthText;

        public PricingPage(IPage page)
        {
            _page = page;

            // These are not the best selectors, but they work (in English) for now. Ideally, for Playwright, we would have data-testid attributes
            // to make tests more robust.
            _popUpCloseButton = _page.FrameLocator("[data-test-id=\"interactive-frame\"]").GetByRole(AriaRole.Button, new() { Name = "Close" });
            _numberOfEmployeesInput = _page.GetByRole(AriaRole.Spinbutton, new() { Name = "Number of employees" });
            _paymentFrequencySelect = _page.GetByLabel("Pay frequency");
            _calculateButton = _page.GetByRole(AriaRole.Button, new() { Name = "Calculate" });
            _costPerMonthText = _page.Locator(".wagepoint-pricing-calculator__result-value");
        }

        public async Task GotoAsync()
        {
            await _page.GotoAsync(Config.Config.BuildUrl("pricing/"));

            // this pop-up seems to always be present, so we'll treat it that way for now. Ideally, we would have a better way to handle this.
            // I tried some alternatives, but they were not reliable. In the real world, I would want to work with the team to find a better solution.
            await _popUpCloseButton.ClickAsync();
        }

        public async Task CalculateCostPerMonth(string numberOfEmployees, PayFrequency payFrequency)
        {
            await _numberOfEmployeesInput.FillAsync(numberOfEmployees);
            await _paymentFrequencySelect.ClickAsync(); // this drop-down requires a click to open before selecting an option
            await _paymentFrequencySelect.SelectOptionAsync(GetPayFrequencyString(payFrequency));
            await _calculateButton.ClickAsync();
        }

        private static string GetPayFrequencyString(PayFrequency payFrequency)
        {
            var type = typeof(PayFrequency);
            var memInfo = type.GetMember(payFrequency.ToString());
            if (memInfo.Length > 0)
            {
                var attrs = memInfo[0].GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
                if (attrs.Length > 0)
                {
                    return ((System.ComponentModel.DescriptionAttribute)attrs[0]).Description;
                }
            }

            return payFrequency.ToString();
        }
    }
}
