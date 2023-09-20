using static Core.Html.Tools.TagsNavigator;
using static Core.Html.Reading.Tags.HtmlTagReader;
using static Core.Html.Reading.Tags.HtmlTagKind;

namespace Core.Html.Reading.Tags;

// HACK: Cyclomatics on TagsCounter!
internal static class HtmlTagExtractor
{
    /// <summary>
    /// Reads html from beginning tag, until the beginning tag is closed.
    /// </summary>
    public static int GetEntireTagLength(ReadOnlySpan<char> html)
    {
        var processed = 0;
        var tagsCounter = new TagsCounter();
        do {
            // Prepare
            processed += GetNextTagIndex(html[processed..]);

            // Process
            Process(html[processed..], tagsCounter);
            
            // Proceed
            processed += GetInnerTextIndex(html[processed..]);

        } while (tagsCounter.HasTags);
        
        return processed;
    }

    private static void Process(ReadOnlySpan<char> currentHtml, TagsCounter processor)
    {
        var tagName = ExtractTagName(currentHtml);
        switch (GetHtmlTagKind(currentHtml))
        {
            case Opening: 
            {
                processor.ProcessOpeningTag(tagName);
                break;
            }
            case Closing:
            {
                processor.ProcessClosingTag(tagName);
                break;
            }
        }
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