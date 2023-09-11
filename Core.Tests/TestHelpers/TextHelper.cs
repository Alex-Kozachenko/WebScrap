using System.Text.RegularExpressions;

namespace Core.Tests.TestHelpers;

public static class TestHelpers
{
    /// <summary>
    /// Strips all excessive symbols and returns space-delimeted string.
    /// </summary>
    internal static string Strip(this string text) 
        => new Regex("\\W")
            .Replace(text, " ")
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Aggregate((aggr, next) => aggr + " " + next);

    internal static string Strip(this ReadOnlySpan<char> text) 
        => text.ToString().Strip();
}