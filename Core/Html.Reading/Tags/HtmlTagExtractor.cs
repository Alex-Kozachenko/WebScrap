using static Core.Html.Tools.TagsNavigator;
using static Core.Html.Reading.Tags.HtmlTagReader;

namespace Core.Html.Reading.Tags;

internal static class HtmlTagExtractor
{
    /// <summary>
    /// Reads html from beginning tag, until the beginning tag is closed.
    /// </summary>
    public static int GetTagLength(ReadOnlySpan<char> html)
    {
        Stack<ReadOnlyMemory<char>> tags = new();
        
        var processed = 0;
        
        do {
            processed += GetNextTagIndex(html[processed..]);

            ReadHtmlTag(html[processed..])
                .StackHtmlTag(tags);
            
            processed += GetInnerTextIndex(html[processed..]);

        } while (tags.Count is not 0);
        
        return processed;
    }

    /// <summary>
    /// Reads html from beginning tag, until the beginning tag is closed.
    /// </summary>
    public static ReadOnlySpan<char> ExtractEntireTag(ReadOnlySpan<char> html)
        => html[..GetTagLength(html)]; // TODO: remove redundancy!
    
    private static void StackHtmlTag(
        this HtmlTag htmlTag, 
        Stack<ReadOnlyMemory<char>> tags)
    {
        if (htmlTag.IsOpening)
        {
            // HACK: it's not designed to call ToArray
            // need to push Span into stack somehow.
            tags.Push(htmlTag.Name.ToArray());
        }
        else 
        {
            if (tags.Pop().Span.SequenceEqual(htmlTag.Name) is not true)
            {
                throw new InvalidOperationException($"""
                    -----
                    Incorrect tag met. 
                    Expected: {tags.Peek()}, 
                    Actual: {htmlTag.Name}. 
                    -----
                """);
            }
        }
    }
}