namespace Core.Tools;

internal readonly ref struct CssTokenizer(Span<char> tokenDelimeters)
{
    private readonly Span<char> tokenDelimeters = tokenDelimeters;

    internal static CssTokenizer Default
        => new([' ', '>']);

    internal Queue<CssToken> TokenizeCss(string css)
        => ToCssTokens(Split(css.AsMemory()));

    private Queue<ReadOnlyMemory<char>> Split(ReadOnlyMemory<char> cssSpan)
    {
        cssSpan = cssSpan.Trim();
        var tokens = new List<ReadOnlyMemory<char>>();
        while (cssSpan.IsEmpty is not true)
        {
            var index = cssSpan
                .Span
                .LastIndexOfAny(tokenDelimeters);

            if (index is not -1)
            {
                tokens.Add(cssSpan[index..]);
                cssSpan = cssSpan[..index];
            }
            else            
            {
                tokens.Add(cssSpan);
                break;
            }
        }

        tokens.Reverse();
        return new(tokens);
    }

    private Queue<CssToken> ToCssTokens(Queue<ReadOnlyMemory<char>> tokens)
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
        return result;
    }        
}