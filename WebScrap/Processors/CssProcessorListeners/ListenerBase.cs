namespace WebScrap.Processors.CssProcessorListeners;

internal abstract class ListenerBase
{
    internal abstract void ProcessOpeningTag(ReadOnlySpan<char> tagName);
    internal abstract void ProcessClosingTag(ReadOnlySpan<char> tagName);
}