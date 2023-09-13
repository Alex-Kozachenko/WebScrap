namespace Core.Internal.HtmlProcessing;

internal static class TextExtractor
{
    internal static string ReadBody(this ReadOnlySpan<char> html)
        => (html.IndexOf('<'), html.IndexOf('>')) switch
        {
            (_, 0) => html[1..].ReadBody(),
            (-1, _) => new string(html), // opening tag not found.
            (0, var close) => html[close..].ReadBody(), // skip opening tag content.
            (var open, var close) // current position is on the middle of a tag.
                when open > close 
                => html[close..].ReadBody(),
            (var open, _) => new string(html[..open]) + ReadBody(html[open..]),
        };
}