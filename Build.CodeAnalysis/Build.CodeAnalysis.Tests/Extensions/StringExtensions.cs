namespace Build.CodeAnalysis.Tests.Extensions
{
    public static class StringExtensions
    {
        public static string ToQuoted(this string value)
        {
            return '"' + value + '"';
        }
    }
}