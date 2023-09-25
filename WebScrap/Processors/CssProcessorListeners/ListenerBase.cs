namespace WebScrap.Processors.CssProcessorListeners;

internal abstract class ListenerBase
{
    protected abstract void ProcessOpeningTag(ReadOnlySpan<char> tagName);
    protected abstract void ProcessClosingTag(ReadOnlySpan<char> tagName);

    internal static void ProcessOpeningTag(
        IEnumerable<ListenerBase> listeners,
        ReadOnlySpan<char> tagName)
    {
        foreach (var listener in listeners)
        {
            listener.ProcessOpeningTag(tagName);
        }
    }

    internal static void ProcessClosingTag(
        IEnumerable<ListenerBase> listeners,
        ReadOnlySpan<char> tagName)
    {
        foreach (var listener in listeners)
        {
            listener.ProcessClosingTag(tagName);
        }
    }
}