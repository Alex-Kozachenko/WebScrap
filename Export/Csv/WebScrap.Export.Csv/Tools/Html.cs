namespace WebScrap.Export.Csv.Tools;

internal static class Html
{
    internal static string Strip(ReadOnlySpan<char> html)
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
                => html[..o].ToString() + Strip(html[c..])
        };
    }

    internal static string SkipTag(ReadOnlySpan<char> html, int tagEnd)
        => Strip(html[1..][tagEnd..]);
}