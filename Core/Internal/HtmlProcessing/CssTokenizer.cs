namespace Core.Internal.HtmlProcessing;

internal readonly ref struct CssTokenizer(Span<char> tokenDelimeters)
{
    private readonly Span<char> tokenDelimeters = tokenDelimeters;

    internal static CssTokenizer Default
        => new([' ', '>']);

    internal Queue<CssToken> TokenizeCss(ReadOnlyMemory<char> css)
        => ToCssTokens(Split(css));

    private Queue<ReadOnlyMemory<char>> Split(ReadOnlyMemory<char> css)
    {
        css = css.Trim();
        var tokens = new List<ReadOnlyMemory<char>>();
        while (css.IsEmpty is not true)
        {
            var lastDelimeterIndex = GetLastDelimeterIndex(css);            
            tokens.Add(css[lastDelimeterIndex..]);
            css = css[..lastDelimeterIndex];
        }
        tokens.Reverse(); 
        return new(tokens);
    }

    private int GetLastDelimeterIndex(ReadOnlyMemory<char> cssLine)
    {
        var delimeterIndex = cssLine
                .Span
                .LastIndexOfAny(tokenDelimeters);

        return delimeterIndex switch
        {
            -1 => 0,
            _ => delimeterIndex
        };
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