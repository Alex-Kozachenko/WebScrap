using WebScrap.Tags;

namespace WebScrap.Processors.Common;

/// <summary>
/// Represents a process of linear html-traversing,
/// with help of <see cref="Processed"/> property.
/// </summary>
/// <remarks>
/// - It is concerned about any html tag, 
/// reacting on opening and closing tag as well.
/// - Highly depends on <see cref="Processed"/> property value, 
/// which is controlled by derived classes.
/// </remarks>
public abstract class ProcessorBase
{
    public int Processed { get; private set; }
    protected abstract bool IsDone { get; }

    protected void Run(ReadOnlySpan<char> html)
    {
        do
        {
            // NOTE: this approach could be confusive for derived classes.
            Processed += Prepare(html[Processed..]);
            Process(html[Processed..]);
            Processed += Proceed(html[Processed..]);
        } while (IsDone is false);
    }

    protected abstract int Prepare(ReadOnlySpan<char> html);

    protected abstract int Proceed(ReadOnlySpan<char> html);

    protected abstract void Process(
        ReadOnlySpan<char> html,
        OpeningTag tag);

    protected abstract void Process(
        ReadOnlySpan<char> html,
        ClosingTag tag);

    private void Process(ReadOnlySpan<char> html)
    {
        if (!html.StartsWith("<"))
        {
            throw new ArgumentException($"Html should start with tag. {html}");
        }
        var tag = TagBase.Create(html);

        if (tag is OpeningTag oTag)
        {
            Process(html, oTag);
        }
        
        if (tag is ClosingTag cTag)
        {
            Process(html, cTag);
        }
    }
}