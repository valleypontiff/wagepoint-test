using NUnit.Framework;

namespace Wagepoint.Tests.Config
{
    internal static class Config
    {
        private static string _baseUrl = "https://www.wagepoint.com";
        private static SaveTraces _saveTraces = SaveTraces.OnFailure;

        public static string BaseUrl => _baseUrl;

        public static SaveTraces SaveTraces => _saveTraces;

        public static string BuildUrl(string relativePath)
        {
            return $"{BaseUrl.TrimEnd('/')}/{relativePath.TrimStart('/')}";
        }

        public static void LoadConfig(TestParameters testParameters)
        {
            _baseUrl = testParameters["baseUrl"]?.ToString() ?? _baseUrl;

            string saveTraces = testParameters["saveTraces"]?.ToString() ?? _saveTraces.ToString();
            if (saveTraces.Equals("always", StringComparison.OrdinalIgnoreCase))
            {
                _saveTraces = SaveTraces.Always;
            }
            else if (saveTraces.Equals("onFailure", StringComparison.OrdinalIgnoreCase))
            {
                _saveTraces = SaveTraces.OnFailure;
            }
            else if (saveTraces.Equals("never", StringComparison.OrdinalIgnoreCase))
            {
                _saveTraces = SaveTraces.Never;
            }
            else
            {
                throw new Exception($"saveTraces is {saveTraces} but must be one of: never, onFailure, always");
            }
        }
    }
}
