using static Core.Html.Reading.Tags.HtmlTagReader;
using static Core.Html.Reading.Tags.HtmlTagKind;
using Core.Common;

namespace Core.Html.Reading.Tags;

internal static class HtmlTagExtractor
{
    /// <summary>
    /// Reads html from beginning tag, until the beginning tag is closed.
    /// </summary>
    public static int GetEntireTagLength(ReadOnlySpan<char> html)
    {
        var processor = new HtmlTagExtractorProcessor();
        processor.Run(html);
        return processor.Processed;
    }

    /// <summary>
    /// Reads html from beginning tag, until the beginning tag is closed.
    /// </summary>
    public static ReadOnlySpan<char> ExtractEntireTag(ReadOnlySpan<char> html)
        => html[..GetEntireTagLength(html)]; // TODO: remove redundancy!

    public static ReadOnlySpan<char> ExtractTagName(ReadOnlySpan<char> html) 
        => GetHtmlTagKind(html) switch
        {
            Opening => html[1..html.IndexOfAny(' ', '>')],
            Closing => html[2..html.IndexOf('>')],
            _ => throw new NotImplementedException()
        };
}