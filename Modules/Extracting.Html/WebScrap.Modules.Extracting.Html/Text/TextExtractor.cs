using WebScrap.Modules.Extracting.Html.Contracts;

namespace WebScrap.Modules.Extracting.Html.Text;

public class TextExtractor : ITextExtractor
{
    public string ExtractText(ReadOnlySpan<char> html)
    {
        var (open, close) = (html.IndexOf('<'), html.IndexOf('>'));
        return (open, close) switch 
        {
            (-1, -1) => html.ToString(),
            (-1, var c) => SkipTag(html, c),
            (0, var c) => SkipTag(html, c),
            (var o, var c) 
                when o > c
                => SkipTag(html, c),
            (var o, var c) 
                => html[..o].ToString() + ExtractText(html[c..])
        };
    }

    internal string SkipTag(ReadOnlySpan<char> html, int tagEnd)
        => ExtractText(html[1..][tagEnd..]);
}