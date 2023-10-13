using System.Collections.Immutable;

namespace WebScrap.Css.Tests.Helpers;

public static class PointersHelper
{
    public static Range[] ToRanges(this string pointers)
        => ToRanges(pointers.AsSpan());

    public static Range[] ToRanges(this ReadOnlySpan<char> pointers)
    {
        var result = new List<Range>();
        var processed = 0;
        while (pointers.IsEmpty is false)
        {
            var end = pointers.IndexOf(' ');
            if (pointers[0] != '^')
            {
                end = 1;
            } 
            else if (end == -1)
            {
                end = pointers.Length;
                result.Add(processed..(end + processed));
            }
            else
            {
                result.Add(processed..(end + processed));
            }
            pointers = pointers[end..];
            processed += end;
        }
        return [.. result];
    }

    public static string[] ToSubstrings(this IEnumerable<int> indexes, string html)
        => indexes.Select(x => html[x..]).ToArray();

    public static string[] ToSubstrings(this IEnumerable<Range> ranges, string html)
        => ranges.Select(x => html[x]).ToArray();
}