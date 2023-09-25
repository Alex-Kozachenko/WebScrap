using WebScrap.Processors.Common;
using WebScrap.Tools.Html;

namespace WebScrap.Processors;

/// <summary>
/// Processes one html tag with its children.
/// </summary>
/// <remarks>
/// - Knows when to stop, 
/// so it will ignore anything beyond targeted tag.
/// </remarks>
public class TagsProcessor : ProcessorBase
{
    private readonly Stack<ReadOnlyMemory<char>> tags = new();
    protected override bool IsDone => tags.Count is 0;

    public static int GetEntireTagLength(ReadOnlySpan<char> html)
    {
        var processor = new TagsProcessor();
        processor.Run(html);
        return processor.Processed;
    }

    public static ReadOnlySpan<char> ExtractEntireTag(ReadOnlySpan<char> html)
    {
        var processor = new TagsProcessor();
        processor.Run(html);
        return html[..processor.Processed];
    }

    protected override int Prepare(ReadOnlySpan<char> html)
        => TagsNavigator.GetNextTagIndex(html);

    protected override int Proceed(ReadOnlySpan<char> html)
    {
        return TagsNavigator.GetInnerTextIndex(html);
    }

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