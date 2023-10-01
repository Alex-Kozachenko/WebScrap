using WebScrap.Common.Tags;

namespace WebScrap.Css.Listeners;

internal static class ListenerBaseExtensions
{
    internal static TListener Get<TListener>(
        this IEnumerable<ListenerBase> listeners)
            where TListener : ListenerBase
        => listeners.OfType<TListener>().First();

    internal static void Process(
        this IEnumerable<ListenerBase> listeners,
        OpeningTag tag)
    {
        foreach (var listener in listeners)
        {
            listener.Process(tag);
        }
    }

    internal static void Process(
        this IEnumerable<ListenerBase> listeners,
        ClosingTag tag)
    {
        foreach (var listener in listeners)
        {
            listener.Process(tag);
        }
    }
}