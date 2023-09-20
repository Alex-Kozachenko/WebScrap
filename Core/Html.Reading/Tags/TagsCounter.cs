using static Core.Html.Reading.Tags.HtmlTagExtractor;

namespace Core.Html.Reading.Tags;

// HACK: Cyclomatics on HtmlTagExtractor!
internal struct TagsCounter()
{
    Stack<ReadOnlyMemory<char>> tags = new();
    internal bool HasTags => tags.Count != 0;

    internal void ProcessOpeningTag(ReadOnlySpan<char> tagName)
    {
        // HACK: it's not designed to call ToArray
        // need to push Span into stack somehow.
        tags.Push(tagName.ToArray());
    }

    internal void ProcessClosingTag(ReadOnlySpan<char> tagName)
    {
        var lastTagName = tags.Pop().Span;
        if (lastTagName.SequenceEqual(tagName) is not true)
        {
            throw new InvalidOperationException($"""
                    -----
                    Incorrect tag met. 
                    Expected: {tags.Peek()}, 
                    Actual: {tagName}. 
                    -----
                """);
        }
    }
}