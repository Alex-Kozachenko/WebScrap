using WebScrap.Common.Tags;
using WebScrap.Common.Tools;
using WebScrap.Core.Tags.Creators;

namespace WebScrap.Core.Tags;

/// <summary>
/// Represents a process of linear html-Matching,
/// with help of <see cref="CharsProcessed"/> property.
/// </summary>
/// <remarks>
/// - It is concerned about any html tag, 
/// reacting on opening and closing tag as well.
/// - Highly depends on <see cref="CharsProcessed"/> property value, 
/// which is controlled by derived classes.
/// </remarks>
public class TagsProcessorBase
{
    private readonly Stack<OpeningTag> tagsHistory = new();
    public int CharsProcessed { get; private set; }
    protected OpeningTag[] TagsHistory => [..tagsHistory.Reverse()];

    private TagFactory tagFactory = new TagFactory();

    public void Run(ReadOnlySpan<char> html)
    {
        CharsProcessed = 0;
        tagsHistory.Clear();
        do
        {
            Process(html[CharsProcessed..]);
            CharsProcessed += Proceed(html[CharsProcessed..]);
        } while (CharsProcessed < html.Length && tagsHistory.Count != 0);
    }

    private int Proceed(ReadOnlySpan<char> html)
        => TagsNavigator.GetNextTagIndex(html[1..]) + 1;

    private void Process(ReadOnlySpan<char> html)
    {
        if (!html.StartsWith("<"))
        {
            throw new ArgumentException($"Html should start with tag. {html}");
        }

        var tag = tagFactory.CreateTagBase(html);
        if (tag is InlineTag iTag)
        {
            return;
        }

        if (tag is OpeningTag oTag)
        {
            tagsHistory.Push(oTag);
            Process(oTag);
        }
        
        if (tag is ClosingTag cTag)
        {
            Process(cTag);
            tagsHistory.Pop();
        }
    }

    protected virtual void Process(OpeningTag tag) { }
    protected virtual void Process(ClosingTag tag) { }
}