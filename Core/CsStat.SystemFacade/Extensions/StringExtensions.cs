namespace CsStat.SystemFacade.Extensions
{
    public static class StringExtensions
    {
        public static bool IsEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        public static int ParseOrDefault(this string value, int defaultValue)
        {
            if (value.IsEmpty())
            {
                return defaultValue;
            }

            return int.TryParse(value, out var parsedValue)
                ? parsedValue
                : defaultValue;
        }
    }
}