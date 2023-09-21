using System.Text;
using static Core.Tools.Html.HtmlValidator;
using static Core.Tools.Html.TagsNavigator;

namespace Core.Processors;

internal class TextProcessor(int htmlLength) : ProcessorBase
{
    private Queue<Range> ranges = new();
    protected override bool IsDone => Processed >= htmlLength;

    public static ReadOnlySpan<char> ExtractText(ReadOnlySpan<char> html)
    {
        var processor = new TextProcessor(html.Length);
        processor.Run(html);
        return processor.ExtractString(html);
    }

    protected override int Prepare(ReadOnlySpan<char> html)
    {
        return 0;
    }

    protected override int Proceed(ReadOnlySpan<char> html)
    {
        return GetNextTagIndexSkipCurrent(html);
    }

    protected override void ProcessOpeningTag(
        ReadOnlySpan<char> html, 
        ReadOnlySpan<char> tagName)
    {
        ProcessTag(html);        
    }

    protected override void ProcessClosingTag(
        ReadOnlySpan<char> html, 
        ReadOnlySpan<char> tagName)
    {
        ProcessTag(html);
    }

    private void ProcessTag(ReadOnlySpan<char> html)
    {
        var range = GetInnerTextIndex(html)..GetNextTagIndexSkipCurrent(html);
        ranges.Enqueue(range);
    }

    private ReadOnlySpan<char> ExtractString(ReadOnlySpan<char> html)
    {
        var sb = new StringBuilder();
        while (ranges.Count is not 0)
        {
            sb.Append(html[ranges.Dequeue()]);
        }
        return sb.ToString();
    }
}