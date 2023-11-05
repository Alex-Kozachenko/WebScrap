using System.Web;
using WebScrap.Modules.Extracting.Html.Contracts;

namespace WebScrap.Modules.Extracting.Html.Text;

public class TextExtractor : ITextExtractor
{
    public string ExtractText(ReadOnlySpan<char> html)
        => ExtractTextInternal(html).Trim();

    string ExtractTextInternal(ReadOnlySpan<char> html)
    {
        var (open, close) = (html.IndexOf('<'), html.IndexOf('>'));
        return (open, close) switch 
        {
            (-1, -1) => Read(html),
            (-1, var c) => SkipTag(html, c),
            (0, var c) => SkipTag(html, c),
            (var o, var c) 
                when o > c
                => SkipTag(html, c),
            (var o, var c) 
                => Read(html[..o]) + ExtractTextInternal(html[c..])
        };
    }

    string Read(ReadOnlySpan<char> html)
        => HttpUtility.HtmlDecode(html.ToString());

    string SkipTag(ReadOnlySpan<char> html, int tagEnd)
        => ExtractTextInternal(html[1..][tagEnd..]);
}