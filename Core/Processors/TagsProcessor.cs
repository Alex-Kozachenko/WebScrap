using Core.Html.Tools;

namespace Core.Processors;

internal class TagsProcessor : ProcessorBase
{
    private Stack<ReadOnlyMemory<char>> tags = new();
    public override bool IsDone => tags.Count is 0;

    public override int Prepare(ReadOnlySpan<char> html)
        => TagsNavigator.GetNextTagIndex(html);

    public override int Proceed(ReadOnlySpan<char> html)
        => TagsNavigator.GetInnerTextIndex(html);

    protected override void ProcessOpeningTag(
        ReadOnlySpan<char> html, 
        ReadOnlySpan<char> tagName)
    {
        tags.Push(tagName.ToArray());
    }

    protected override void ProcessClosingTag(
        ReadOnlySpan<char> html, 
        ReadOnlySpan<char> tagName)
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