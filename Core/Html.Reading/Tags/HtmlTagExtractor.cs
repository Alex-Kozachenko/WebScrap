using Core.Processors;

namespace Core.Html.Reading.Tags;

// TODO: delete.
internal static class HtmlTagExtractor
{
    /// <summary>
    /// Reads html from beginning tag, until the beginning tag is closed.
    /// </summary>
    public static int GetEntireTagLength(ReadOnlySpan<char> html)
    {
        var processor = new TagsProcessor();
        processor.Run(html);
        return processor.Processed;
    }

    /// <summary>
    /// Reads html from beginning tag, until the beginning tag is closed.
    /// </summary>
    public static ReadOnlySpan<char> ExtractEntireTag(ReadOnlySpan<char> html)
        => html[..GetEntireTagLength(html)]; // TODO: remove redundancy!
}