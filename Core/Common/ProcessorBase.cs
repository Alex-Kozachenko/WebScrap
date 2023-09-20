using Core.Html.Reading.Tags;
using Core.Html.Tools;


namespace Core.Common;

internal class HtmlTagExtractorProcessor2
{

}

// TODO: rename, its just a proto.
internal class HtmlTagExtractorProcessor()
{
    public int Processed { get; private set; } = 0;
    private TagsCounter processor = new TagsCounter();

    public void Run(ReadOnlySpan<char> html)
    {
        do
        {
            Prepare(html[Processed..]);
            Process(html[Processed..]);
            Proceed(html[Processed..]);
        } while (IsDone() is false);
    }

    public void Prepare(ReadOnlySpan<char> html)
    {
        Processed += TagsNavigator.GetNextTagIndex(html);
    }

    public void Process(ReadOnlySpan<char> html)
    {
        var tagName = HtmlTagExtractor.ExtractTagName(html);
        switch (HtmlTagReader.GetHtmlTagKind(html))
        {
            case HtmlTagKind.Opening:
            {
                processor.ProcessOpeningTag(tagName);
                OnOpeningTagProcessed();
                break;
            }
            case HtmlTagKind.Closing:
            {
                processor.ProcessClosingTag(tagName);
                OnClosingTagProcessed();
                break;
            }
        }
    }

    public void Proceed(ReadOnlySpan<char> html)
    {
        Processed += TagsNavigator.GetInnerTextIndex(html);
    }

    public bool IsDone() 
        => processor.HasTags is false;

    protected virtual void OnOpeningTagProcessed() { }

    protected virtual void OnClosingTagProcessed() { }

}