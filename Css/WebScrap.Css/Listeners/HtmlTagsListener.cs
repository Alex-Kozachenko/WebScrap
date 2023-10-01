using WebScrap.Common.Tags;

namespace WebScrap.Css.Listeners;

/// <summary>
/// Tracks all tags met.
/// </summary>
internal class HtmlTagsListener : ListenerBase
{
    private readonly Stack<OpeningTag> traversedTags = new();

    public Stack<OpeningTag> TraversedTags => new(traversedTags.Reverse());

    internal override void Process(OpeningTag tag)
    {
        traversedTags.Push(tag);
    }

    internal override void Process(ClosingTag tag)
    {
        traversedTags.Pop();
    }
}