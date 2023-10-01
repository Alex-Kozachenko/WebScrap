namespace WebScrap.Css.Attributes;

/// <summary>
/// Reads a lookup of css tokens from css-like query.
/// </summary>
internal class Reader(ReadOnlyMemory<char> cssToken)
{
    private int CurrentTokenIndex
        => cssToken.Span.LastIndexOfAny(".#");

    public ILookup<string, string> Read()
    {
        var result = new List<(string Key, string Value)>();
        while (cssToken.Length != 0)
        {
            result.Add(new(ReadKey(), ReadValue()));
            cssToken = Proceed();
        }
        result.Reverse();
        return result.ToLookup(x => x.Key, x => x.Value);
    }

    private string ReadKey()
        => cssToken.Span[CurrentTokenIndex] switch
        {
            '#' => "id",
            '.' => "class",
            _ => throw new ArgumentException($"Unknown attribute met: {cssToken}")
        };

    private string ReadValue()
        => cssToken[1..][CurrentTokenIndex..].ToString();

    private ReadOnlyMemory<char> Proceed()
        => cssToken[..CurrentTokenIndex];
}