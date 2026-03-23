using System.ComponentModel;

namespace Wagepoint.Tests.Enums
{
    // This enum represents the pay frequency options available in the wagepoint pricing calculator.
    // The Description attributes map to value attributes for selecting the correct option during testing.
    public enum PayFrequency
    {
        [Description("select")]
        Select,
        [Description("weekly")]
        Weekly,
        [Description("biweekly")]
        BiWeekly,
        [Description("semimonthly")]
        SemiMonthly,
        [Description("monthly")]
        Monthly
    }
}
