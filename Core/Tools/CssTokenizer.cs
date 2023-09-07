namespace Core.Tools;

internal readonly ref struct CssTokenizer(Span<char> tokenDelimeters)
{
    private readonly Span<char> tokenDelimeters = tokenDelimeters;

    internal static CssTokenizer Default
        => new([' ', '>']);

    internal Queue<CssToken> TokenizeCss(string css)
        => ToCssTokens(Split(css.AsMemory()));

    private Queue<ReadOnlyMemory<char>> Split(ReadOnlyMemory<char> cssLine)
    {
        cssLine = cssLine.Trim();
        var tokens = new List<ReadOnlyMemory<char>>();
        while (cssLine.IsEmpty is not true)
        {
            var lastDelimeterIndex = GetLastDelimeterIndex(cssLine);            
            tokens.Add(cssLine[lastDelimeterIndex..]);
            cssLine = cssLine[..lastDelimeterIndex];
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