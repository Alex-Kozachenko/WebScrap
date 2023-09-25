namespace WebScrap.Processors.CssProcessorListeners;

internal class TraversedTagsListener : ListenerBase
{
    private readonly Stack<string> traversedTags = new();

    public Stack<string> TraversedTags => new(traversedTags.Reverse());

    protected override void ProcessOpeningTag(ReadOnlySpan<char> tagName)
    {
        traversedTags.Push(tagName.ToString());
    }

    protected override void ProcessClosingTag(ReadOnlySpan<char> tagName)
    {
        traversedTags.Pop();
    }
}