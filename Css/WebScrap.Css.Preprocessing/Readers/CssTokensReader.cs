using System.Collections.Immutable;
using WebScrap.Css.Common;

namespace WebScrap.Css.Preprocessing.Readers;

// HACK: why public? 
public static class CssTokensReader
{
    private static readonly char[] childSelectors = [' ', '>'];

    public static ImmutableArray<CssToken> Read(ReadOnlySpan<char> css)
        => css.Tokenize().Read();

    private static ImmutableArray<ReadOnlyMemory<char>> Tokenize(
        this ReadOnlySpan<char> css)
    {
        css = css.Trim();
        var tokens = new List<ReadOnlyMemory<char>>();
        while (css.IsEmpty is not true)
        {
            var lastDelimeterIndex = GetLastDelimeterIndex(css);
            var token = css[lastDelimeterIndex..].ToString().AsMemory();
            tokens.Add(token);
            css = css[..lastDelimeterIndex];
        }
        tokens.Reverse();
        return [.. tokens];
    }

    internal static ImmutableArray<CssToken> Read(
        this ImmutableArray<ReadOnlyMemory<char>> tokens)
    {
        var result = new Queue<CssToken>();
        foreach (var token in tokens)
        {
            var cssToken = CssTokenBuilder.Build(token);
            result.Enqueue(cssToken);
        }
        return [.. result];
    }

    private static int GetLastDelimeterIndex(ReadOnlySpan<char> cssLine)
    {
        var delimeterIndex = cssLine
                .LastIndexOfAny(childSelectors);

        return delimeterIndex switch
        {
            -1 => 0,
            _ => delimeterIndex
        };
    }
}