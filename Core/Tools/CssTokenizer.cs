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
        var tokens = new Queue<ReadOnlyMemory<char>>();
        while (cssSpan.IsEmpty is not true)
        {
            var index = cssSpan
                .Span[1..] // skip 1st char, since it could be a delimeter.
                .IndexOfAny(tokenDelimeters);

            if (index == -1)
            {
                tokens.Enqueue(cssSpan);
                break;
            }
            else
            {
                // TODO: MAYBE it's better to process the string in reverse order?
                index++; // encount skipped char, previously.
                tokens.Enqueue(cssSpan[..index]);
                cssSpan = cssSpan[index..];
            }
        }

        return tokens;
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