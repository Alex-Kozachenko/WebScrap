using WebScrap.Common.Tags;

namespace WebScrap.Common.Processors;

internal static class IProcessorListenerExtensions
{
    internal static TListener Get<TListener>(
        this IEnumerable<IProcessorListener> listeners)
            where TListener : IProcessorListener
        => listeners.OfType<TListener>().First();

    
}