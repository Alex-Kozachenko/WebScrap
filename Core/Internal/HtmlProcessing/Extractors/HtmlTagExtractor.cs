using static Core.Internal.HtmlProcessing.TagsLocator;

namespace Core.Internal.HtmlProcessing.Extractors;

internal static class HtmlTagExtractor
{
    /// <summary>
    /// Reads html from beginning tag, until the beginning tag is closed.
    /// </summary>
    public static ReadOnlySpan<char> ExtractTag(ReadOnlySpan<char> html)
    {
        Stack<ReadOnlyMemory<char>> tags = new();
        
        var processed = 0;
        
        do {
            processed += GetNextTagIndex(html[processed..]);
            processed += ProcessTagName(html[processed..], tags);
            
        } while (tags.Count is not 0);
        
        return html[..processed];
    }

    private static int ProcessTagName(
        ReadOnlySpan<char> html, 
        Stack<ReadOnlyMemory<char>> tags)
    {
        var htmlTag = HtmlTagReader.ReadHtmlTag(html);
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
                    {html}
                """);
            }
        }

        const int openingTagOffset = 1;
        const int countingOffset = 1;
        return html[1..].IndexOf('>') // HACK: stinks.
            + openingTagOffset 
            + countingOffset;
    }
}