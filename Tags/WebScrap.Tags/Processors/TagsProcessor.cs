using WebScrap.Common.Processors;
using WebScrap.Common.Tags;
using WebScrap.Common.Tags.Creators;
using WebScrap.Tags.Creators;
using WebScrap.Tags.Tools;

namespace WebScrap.Tags.Processors;

/// <summary>
/// Processes one html tag with its children.
/// </summary>
/// <remarks>
/// - Knows when to stop, 
/// so it will ignore anything beyond targeted tag.
/// </remarks>
public class TagsProcessor(TagFactoryBase tagFactory) 
    : ProcessorBase(tagFactory)
{
    private readonly Stack<ReadOnlyMemory<char>> tags = new();
    protected override bool IsDone => tags.Count is 0;

    public static int GetEntireTagLength(
        TagFactoryBase tagFactory, 
        ReadOnlySpan<char> html)
    {
        var processor = new TagsProcessor(tagFactory);
        processor.Run(html);
        return processor.Processed;
    }

    public static ReadOnlySpan<char> ExtractEntireTag(
        TagFactoryBase tagFactory,
        ReadOnlySpan<char> html)
    {
        var processor = new TagsProcessor(tagFactory);
        processor.Run(html);
        return html[..processor.Processed];
    }

    protected override int Prepare(ReadOnlySpan<char> html)
        => TagsNavigator.GetNextTagIndex(html);

    protected override int Proceed(ReadOnlySpan<char> html) 
        => TagsNavigator.GetInnerTextIndex(html);

    protected override void Process(
        ReadOnlySpan<char> html, 
        OpeningTag tag)
    {
        tags.Push(tag.Name.ToArray());
    }

    protected override void Process(
        ReadOnlySpan<char> html, 
        ClosingTag tag)
    {
        var lastTagName = tags.Pop().Span;
        if (lastTagName.SequenceEqual(tag.Name) is not true)
        {
            throw new InvalidOperationException($"""
                    -----
                    Incorrect tag met. 
                    Expected: {tags.Peek()}, 
                    Actual: {tag.Name}. 
                    -----
                """);
        }
    }
}