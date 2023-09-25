namespace WebScrap.Processors.CssProcessorListeners;

internal static class ListenerBaseExtensions
{
    internal static TListener Get<TListener>(
        this IEnumerable<ListenerBase> listeners)
            where TListener : ListenerBase
        => listeners.OfType<TListener>().First();

    internal static void ProcessOpeningTag(
        this IEnumerable<ListenerBase> listeners,
        ReadOnlySpan<char> tagName)
    {
        foreach (var listener in listeners)
        {
            listener.ProcessOpeningTag(tagName);
        }
    }

    internal static void ProcessClosingTag(
        this IEnumerable<ListenerBase> listeners,
        ReadOnlySpan<char> tagName)
    {
        foreach (var listener in listeners)
        {
            listener.ProcessClosingTag(tagName);
        }
    }
}