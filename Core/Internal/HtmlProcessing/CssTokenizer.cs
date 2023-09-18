using System.Collections.Immutable;

namespace Core.Internal.HtmlProcessing;

internal static class CssTokenizer
{
    private static readonly char[] tokenDelimeters = [' ', '>'];

    public static ImmutableArray<CssToken> TokenizeCss(ReadOnlySpan<char> css)
        => ToCssTokens(Split(css));

    private static ImmutableArray<ReadOnlyMemory<char>> Split(ReadOnlySpan<char> css)
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
        return [..tokens];
    }

    private static ImmutableArray<CssToken> ToCssTokens(
        ImmutableArray<ReadOnlyMemory<char>> tokens)
    {
        var result = new Queue<CssToken>();
        foreach (var token in tokens)
        {
            var firstChar = token.Span[0];
            CssToken item = tokenDelimeters.Contains(firstChar)
                ? new(firstChar, token[1..])
                : new(null, token);
            result.Enqueue(item);
        }
        return [..result];
    }
    
    private static int GetLastDelimeterIndex(ReadOnlySpan<char> cssLine)
    {
        var delimeterIndex = cssLine
                .LastIndexOfAny(tokenDelimeters);

        return delimeterIndex switch
        {
            -1 => 0,
            _ => delimeterIndex
        };
    } 
}