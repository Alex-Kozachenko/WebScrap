using WebScrap.Css.Data.Attributes;

namespace WebScrap.Css.Preprocessing.Readers;

internal ref struct CssAttributesReader
{
    private ReadOnlySpan<char> css;

    internal CssAttributesReader(ReadOnlySpan<char> css)
    {
        ThrowIfUnsupported(css);
        this.css = css;
    }

    public static int Read(ReadOnlySpan<char> css, out CssAttributesLookup attributes)
    {
        attributes = [];
        return css.IndexOfAny(".#") switch
        {
            -1 => 0,
            var i => new CssAttributesReader(css[i..])
                .Read(out attributes)
        };
    }

    private int Read(out CssAttributesLookup attributes)
    {
        var processed = css.Length;
        var result = new List<KeyValuePair<string, string>>();
        while (css.Length != 0)
        {
            var currentTokenIndex = css.LastIndexOfAny(".#");
            var attribute = css[currentTokenIndex..];
            result.Add(ReadAttribute(attribute));
            css = css[..currentTokenIndex];
        }
        result.Reverse();
        attributes = new(result);
        return processed;
    }

    private static KeyValuePair<string, string> ReadAttribute(ReadOnlySpan<char> attr)
        => new(ReadSelector(attr[0]),  attr[1..].ToString());

    private static string ReadSelector(char selector)
        => selector switch
        {
            '#' => "id",
            '.' => "class",
            _ => throw new ArgumentException($"Unknown attribute met: {selector}")
        };

    private static void ThrowIfUnsupported(ReadOnlySpan<char> css)
    {
        _ = css.IndexOfAny("[],") switch 
        {
            -1 => 0,
            var i => throw new ArgumentException($"Unable to process css. Css contains unsupported chars: {css[i..]}.")
        };
    }
}