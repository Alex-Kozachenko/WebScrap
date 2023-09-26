using System.Collections.Immutable;

namespace WebScrap.Tools.Css;

internal static class CssTokenizer
{
    private static readonly char[] childSelectors = [' ', '>'];

    public static ImmutableArray<CssToken> TokenizeCss(ReadOnlySpan<char> css)
        => css.Tokenize().ToCssTokens();

    private static ImmutableArray<ReadOnlyMemory<char>> Tokenize(
        this ReadOnlySpan<char> css)
    {
        css = css.Trim();
        var tokens = new List<ReadOnlyMemory<char>>();
        while (css.IsEmpty is not true)
        {
            var lastDelimeterIndex = GetLastDelimeterIndex(css);
            var token = new ReadOnlyMemory<char>(css[lastDelimeterIndex..].ToArray());
            tokens.Add(token);
            css = css[..lastDelimeterIndex];
        }
        tokens.Reverse();
        return [.. tokens];
    }

    private static ImmutableArray<CssToken> ToCssTokens(
        this ImmutableArray<ReadOnlyMemory<char>> tokens)
    {
        var result = new Queue<CssToken>();
        foreach (var token in tokens)
        {
            result.Enqueue(CssTokenBuilder.Build(token, childSelectors));
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