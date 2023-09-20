using Core.Common;

namespace Core.Html.Reading.Tags;

internal struct TagsCounter() : IProcessor
{
    Stack<ReadOnlyMemory<char>> tags = new();
    internal bool HasTags => tags.Count != 0;

    public void ProcessOpeningTag(ReadOnlySpan<char> tagName)
    {
        tags.Push(tagName.ToArray());
    }

    public void ProcessClosingTag(ReadOnlySpan<char> tagName)
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