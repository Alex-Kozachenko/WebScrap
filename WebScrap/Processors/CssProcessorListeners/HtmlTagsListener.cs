namespace WebScrap.Processors.CssProcessorListeners;

/// <summary>
/// Tracks all tags met.
/// </summary>
internal class HtmlTagsListener : ListenerBase
{
    private readonly Stack<string> traversedTags = new();

    public Stack<string> TraversedTags => new(traversedTags.Reverse());

    internal override void ProcessOpeningTag(ReadOnlySpan<char> tagName)
    {
        traversedTags.Push(tagName.ToString());
    }

    internal override void ProcessClosingTag(ReadOnlySpan<char> tagName)
    {
        traversedTags.Pop();
    }
}