using static Core.Html.Tools.HtmlValidator;
using static Core.Html.Tools.TagsNavigator;
using static Core.Html.Reading.Tags.HtmlTagReader;
using System.Collections.Immutable;
using static Core.Html.Reading.Tags.HtmlTagKind;
using static Core.Html.Reading.Tags.HtmlTagExtractor;

namespace Core;

// NOTE: it seems a tiny tags locator became a center of the project
// TODO: decouple this.
public static class TagsLocator
{
    /// <summary>
    /// Locates the html-tags by css-like string.
    /// </summary>
    /// <returns>
    /// Ranges for tags.
    /// </returns>
    public static ImmutableArray<Range> LocateTagRanges(
        ReadOnlySpan<char> html,
        ReadOnlySpan<char> css)
    {
        html = ToValidHtml(html);
        var processor = new CssProcessor(css);
        List<Range> result = [];
        var processed = 0;
        var originalRange = ..html.Length;

        while (html.Length is not 0)
        {
            // Prepare 
            // Process
            Process(html, processor, processed, result);
            // Proceed
            var nextTagIndex = GetNextTagIndex(html[1..]) + 1;
            processed += html[..nextTagIndex].Length;
            html = html[nextTagIndex..];
        }

        return [.. result];
    }

    private static void Process(
        ReadOnlySpan<char> currentHtml, 
        CssProcessor processor,
        int processed,
        List<Range> result)
    {
        var tagName = ExtractTagName(currentHtml);
        switch (GetHtmlTagKind(currentHtml))
        {
            case Opening: 
            {
                processor.ProcessOpeningTag(tagName);
                if (processor.IsCssCompleted)
                {
                    var tagLength = GetEntireTagLength(currentHtml);
                    var bodyRange = processed..(processed + tagLength);
                    result.Add(bodyRange);
                }
                break;
            }
            case Closing:
            {
                processor.ProcessClosingTag(tagName);
                break;
            }
        }
    }
}