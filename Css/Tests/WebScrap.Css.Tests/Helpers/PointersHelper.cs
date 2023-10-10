using System.Collections.Immutable;

namespace  WebScrap.Css.Tests.Helpers;

public static class PointersHelper
{
    public static int[] ToIndexes(this string pointers)
    {
        var result = new List<int>();
        for (var i = 0; i < pointers.Length; i++)
        {
            var ch = pointers[i];
            if (ch == '^')
            {
                result.Add(i);
            }
        }
        return [.. result];
    }

    public static string[] ToSubstrings(this int[] indexes, string html)
        => indexes.Select(x => html[x..]).ToArray();

    public static string[] ToSubstrings(this ImmutableArray<int> indexes, string html)
        => indexes.Select(x => html[x..]).ToArray();
}