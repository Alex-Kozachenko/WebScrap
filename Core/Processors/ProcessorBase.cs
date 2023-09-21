namespace Core.Processors;

internal abstract class ProcessorBase
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

    protected abstract void ProcessOpeningTag(
        ReadOnlySpan<char> html,
        ReadOnlySpan<char> tagName);

    protected abstract void ProcessClosingTag(
        ReadOnlySpan<char> html,
        ReadOnlySpan<char> tagName);

    private void Process(ReadOnlySpan<char> html)
    {
        var tagName = ExtractTagName(html);
        var kind = GetHtmlTagKind(html);
        if (kind == HtmlTagKind.Opening)
        {
            ProcessOpeningTag(html, tagName);
        }
        else if (kind == HtmlTagKind.Closing)
        {
            ProcessClosingTag(html, tagName);
        }
    }

    private static ReadOnlySpan<char> ExtractTagName(ReadOnlySpan<char> html) 
        => GetHtmlTagKind(html) switch
        {
            HtmlTagKind.Opening => html[1..html.IndexOfAny(' ', '>')],
            HtmlTagKind.Closing => html[2..html.IndexOf('>')],
            _ => throw new NotImplementedException()
        };

    private static HtmlTagKind GetHtmlTagKind(
        ReadOnlySpan<char> html)
    {
        return (html[0], html[1]) switch
        {
            ('<', '/') => HtmlTagKind.Closing,
            ('<', _) => HtmlTagKind.Opening,
            _ => throw new ArgumentException($"Html doesnt start with tag. {html}")
        };
    }


    // TODO: remove.
    private enum HtmlTagKind
    {
        Opening,
        Closing
    }
}