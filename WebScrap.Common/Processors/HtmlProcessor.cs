using WebScrap.Common.Tags;
using WebScrap.Common.Tags.Creators;
using WebScrap.Common.Tools;

namespace WebScrap.Common.Processors;

/// <summary>
/// Represents a process of linear html-traversing,
/// with help of <see cref="CharsProcessed"/> property.
/// </summary>
/// <remarks>
/// - It is concerned about any html tag, 
/// reacting on opening and closing tag as well.
/// - Highly depends on <see cref="CharsProcessed"/> property value, 
/// which is controlled by derived classes.
/// </remarks>
public sealed class HtmlProcessor(
    TagFactoryBase tagFactory, 
    IEnumerable<IProcessorListener> listeners)
{
    private readonly Stack<OpeningTag> tagsHistory = new();
    public int CharsProcessed { get; private set; }
    public IEnumerable<IProcessorListener> Listeners => listeners;

    public void Run(ReadOnlySpan<char> html)
    {
        do
        {
            Process(html[CharsProcessed..]);
            CharsProcessed += Proceed(html[CharsProcessed..]);
        } while (CharsProcessed < html.Length && tagsHistory.Count != 0);
    }

    public TListener GetListener<TListener>()
            where TListener : IProcessorListener
        => listeners.OfType<TListener>().First();

    private int Proceed(ReadOnlySpan<char> html)
        => TagsNavigator.GetNextTagIndex(html[1..]) + 1;

    private void Process(ReadOnlySpan<char> html)
    {
        if (!html.StartsWith("<"))
        {
            throw new ArgumentException($"Html should start with tag. {html}");
        }

        var tag = tagFactory.CreateTagBase(html);
        if (tag is InlineTag)
        {
            return;
        }

        if (tag is OpeningTag oTag)
        {
            tagsHistory.Push(oTag);
            Process(listeners, oTag);
        }
        
        if (tag is ClosingTag cTag)
        {
            Process(listeners, cTag);
            tagsHistory.Pop();
        }
    }

    private void Process(
        IEnumerable<IProcessorListener> listeners,
        OpeningTag tag)
    {
        foreach (var listener in listeners)
        {
            listener.Process(tagsHistory.Reverse().ToArray(), tag);
        }
    }

    private void Process(
        IEnumerable<IProcessorListener> listeners,
        ClosingTag tag)
    {
        foreach (var listener in listeners)
        {
            listener.Process(tagsHistory.Reverse().ToArray(), tag);
        }
    }
}