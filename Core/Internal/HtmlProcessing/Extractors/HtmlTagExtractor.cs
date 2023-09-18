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
        => TryProcessClosingTagName(html, tags)
        ?? TryProcessOpeningTagName(html, tags) 
        ?? throw new ArgumentException($"html doesn't start with tag! {html}");

    private static int? TryProcessOpeningTagName(
        ReadOnlySpan<char> html, 
        Stack<ReadOnlyMemory<char>> tags)
    {
        if (html.StartsWith("<") is not true)
        {
            return null;
        }

        var openingTag = html[1..html.IndexOfAny(' ', '>')];
        // HACK: it's not designed to call ToArray
        // need to push Span into stack somehow.
        tags.Push(openingTag.ToArray());
        return openingTag.Length + 2; // <>
    }

    private static int? TryProcessClosingTagName(
        ReadOnlySpan<char> html, 
        Stack<ReadOnlyMemory<char>> tags)
    {
        if (html.StartsWith("</") is not true)
        {
            return null;
        }

        var closingTag = html[2..html.IndexOf('>')];
        if (tags.Peek().Span.SequenceEqual(closingTag) is not true)
        {
            throw new InvalidOperationException($"""
                -----
                Incorrect tag met. 
                Expected: {tags.Peek()}, 
                Actual: {closingTag}. 
                -----
                {html}
            """);
        }

        tags.Pop();
        return closingTag.Length + 3; // </>
    }
}