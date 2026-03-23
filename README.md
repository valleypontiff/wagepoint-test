# Wagepoint UI Tests

Automated Playwright UI tests for the Wagepoint pricing calculator using NUnit.

## Project structure

- `Wagepoint.Tests/` - NUnit test project containing Playwright page objects and tests.

Key files:
- `Wagepoint.Tests/Pages/PricingPage.cs` - Page object for the pricing calculator.
- `Wagepoint.Tests/PricingTests.cs` - Parametrized tests for the calculator.
- `Wagepoint.Tests/TestBase.cs` - Test fixture base that starts/stops Playwright tracing.
- `Wagepoint.Tests/Config/Config.cs` - Simple configuration loader (test parameters).

## Requirements

- .NET 10 SDK
- Playwright browsers installed (see Setup)
- Visual Studio 2022+ or VS Code / CLI tooling

## Setup

1. Restore and build the solution:

   ```powershell
   dotnet restore
   dotnet build
   ```

2. Install Playwright CLI (one-time):

   playwright.ps1 can be found in the build output directory.

   ```powershell
   dotnet tool install --global Microsoft.Playwright.CLI
   playwright.ps1 install
   ```

   Alternatively, if you have Node.js installed you can run `npx playwright install`.

## Running tests

From the command line (recommended):

```powershell
dotnet test ./Wagepoint.Tests/Wagepoint.Tests.csproj
```

You can run the test project from Visual Studio Test Explorer as well.

### Passing configuration

The test project reads NUnit test parameters via `TestContext.Parameters`. Two parameters are supported:

- `baseUrl` - base URL to test (default: `https://www.wagepoint.com`)
- `saveTraces` - controls Playwright trace saving. Allowed values: `never`, `onFailure`, `always` (default: `onFailure`)

Example `.runsettings` you can select in Visual Studio (or supply to `dotnet test` with `--settings`):

```xml
<?xml version="1.0" encoding="utf-8"?>
<RunSettings>
  <NUnit>
    <TestParameters>
      <Parameter name="baseUrl" value="https://staging.example.com" />
      <Parameter name="saveTraces" value="always" />
    </TestParameters>
  </NUnit>
</RunSettings>
```

Note: you can also use `nunit3-console` and pass parameters using `--params "baseUrl=https://...;saveTraces=onFailure"`.

## Traces and diagnostics

When enabled, Playwright traces are written under the test run work directory in a `traces` folder. Trace files are named after the test full name (sanitized) and saved as `.zip` files when `saveTraces` is `always` or when a test fails and `saveTraces` is `onFailure`.

If you see missing trace files or exceptions when saving traces, ensure the test runner process has write permissions to the test run work directory.

## Notes and recommendations

- The page object selectors are English-language and may be fragile; consider adding stable `data-testid` attributes in the application under test to make selectors robust.
- Tests are marked with NUnit parallelization; ensure any external resources (files, trace outputs) do not conflict when running tests in parallel.
- Extreme numeric inputs in `PricingTests` are intentional exploratory cases; adjust or remove them if the application under test cannot reasonably handle extremely large values.

## Troubleshooting

- If browsers are not launching, re-run `playwright install`.
- If tests fail due to missing test parameters, add a `.runsettings` or pass parameters via your test runner.

## Contributing

Open a PR with changes to tests or page objects. Keep tests deterministic and isolate external dependencies where possible.

---
Generated for the `Wagepoint.Tests` project.
