using WebScrap.Css.Common.Attributes;

namespace WebScrap.Css.Preprocessing.Readers;

internal ref struct CssAttributesReader
{
    private static readonly char[] delimeters = [' ', '>'];
    private ReadOnlySpan<char> css;

    internal CssAttributesReader(ReadOnlySpan<char> css)
    {
        ThrowIfUnsupported(css);
        this.css = css;
    }

    private readonly int CurrentTokenIndex
        => css.LastIndexOfAny(".#");

    public static int Read(ReadOnlySpan<char> css, out CssAttributesLookup attributes)
    {
        var tokenDelimeterIndex = css.LastIndexOfAny("> ");
        if (tokenDelimeterIndex != -1)
        {
            return Read(css[tokenDelimeterIndex..][1..], out attributes);
        }

        var attributeDelimeterIndex = css.IndexOfAny(".#");
        if (attributeDelimeterIndex == -1)
        {
             attributes = [];
             return 0;
        }

        return new CssAttributesReader(css[attributeDelimeterIndex..])
            .Read(out attributes);
    }

    private int Read(out CssAttributesLookup attributes)
    {
        var processed = css.Length;
        var result = new List<KeyValuePair<string, string>>();
        while (css.Length != 0)
        {
            result.Add(new(ReadSelector(), ReadTarget()));
            css = css[..CurrentTokenIndex];
        }
        result.Reverse();
        attributes = new(result);
        return processed;
    }

    private readonly string ReadSelector()
        => css[CurrentTokenIndex] switch
        {
            '#' => "id",
            '.' => "class",
            _ => throw new ArgumentException($"Unknown attribute met: {css}")
        };

    private readonly string ReadTarget()
        => css[CurrentTokenIndex..][1..].ToString();

    private static void ThrowIfUnsupported(ReadOnlySpan<char> css)
    {
        _ = css.IndexOfAny("[],") switch 
        {
            -1 => 0,
            var i => throw new ArgumentException($"Unable to process css. Css contains unsupported chars: {css[i..]}.")
        };
    }
}