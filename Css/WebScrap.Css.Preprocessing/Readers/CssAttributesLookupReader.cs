using WebScrap.Css.Common.Attributes;

namespace WebScrap.Css.Preprocessing.Readers;

internal class AttributesReader(ReadOnlyMemory<char> cssToken)
{
    private int CurrentTokenIndex
        => cssToken.Span.LastIndexOfAny(".#");

    public CssAttributesLookup Read()
    {
        var result = new List<KeyValuePair<string, string>>();
        while (cssToken.Length != 0)
        {
            result.Add(new(ReadKey(), ReadValue()));
            cssToken = Proceed();
        }
        result.Reverse();
        return new(result);
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