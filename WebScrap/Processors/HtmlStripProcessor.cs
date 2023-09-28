using WebScrap.Processors.Common;
using System.Text;
using static WebScrap.Tools.Html.TagsNavigator;
using WebScrap.Tags;

namespace WebScrap.Processors;

/// <summary>
/// Extracts plain text from html, 
/// removing any technical chars inside.
/// Basically, stripping the html.
/// </summary>
public class HtmlStripProcessor(int htmlLength) : ProcessorBase
{
    private readonly Queue<Range> ranges = [];
    protected override bool IsDone => Processed >= htmlLength;

    public static ReadOnlySpan<char> ExtractText(ReadOnlySpan<char> html)
    {
        var processor = new HtmlStripProcessor(html.Length);
        processor.Run(html);
        return processor.ExtractString(html);
    }

    protected override int Prepare(ReadOnlySpan<char> html) 
        => 0;

    protected override int Proceed(ReadOnlySpan<char> html)
    {
        return GetNextTagIndexSkipCurrent(html);
    }

    protected override void Process(
        ReadOnlySpan<char> html, 
        OpeningTag tag)
    {
        ProcessTag(tag.InnerText);
    }

    protected override void Process(
        ReadOnlySpan<char> html, 
        ClosingTag tag)
    {
        
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