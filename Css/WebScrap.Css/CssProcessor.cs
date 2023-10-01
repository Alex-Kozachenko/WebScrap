using System.Collections.Immutable;
using WebScrap.Css.Listeners;
using WebScrap.Tags;
using WebScrap.Tags.Processors;
using WebScrap.Tags.Tools;

namespace WebScrap.Css;

/// <summary>
/// Processes the html with provided css-like-selectors,
/// and returns detected html which conforms 
/// the css from the parameter.
/// </summary>
public class CssProcessor : ProcessorBase
{
    private readonly List<int> tagIndexes = [];
    private readonly ListenerBase[] listeners;
    private readonly int htmlLength;
    protected override bool IsDone => Processed >= htmlLength;

    public CssProcessor(
        ReadOnlySpan<char> css,
        int htmlLength)
    {
        var tagsListeners = new {
            css = new CssTagsListener(css),
            tags = new HtmlTagsListener()
        };
        tagsListeners.css.Completed += OnCompletedCssMet;
        // HACK: the order matters, since css has the event.
        listeners = [tagsListeners.tags, tagsListeners.css];

        this.htmlLength = htmlLength;
    }

    public static ImmutableArray<int> CalculateTagIndexes(
        ReadOnlySpan<char> html,
        ReadOnlySpan<char> css)
    {
        var processor = new CssProcessor(css, html.Length);
        processor.Run(html);
        return [.. processor.tagIndexes];
    }

    protected override int Prepare(ReadOnlySpan<char> html)
        => 0;

    protected override int Proceed(ReadOnlySpan<char> html)
        => TagsNavigator.GetNextTagIndex(html[1..]) + 1;

    protected override void Process(
        ReadOnlySpan<char> html,
        OpeningTag tag)
    {
        listeners.Process(tag);
    }

    protected override void Process(
        ReadOnlySpan<char> html,
        ClosingTag tag)
    {
        listeners.Process(tag);
    }

    private void OnCompletedCssMet(object? sender, EventArgs args)
    {
        if (IsCssComplied())
        {
            tagIndexes.Add(Processed);
        }
    }

    private bool IsCssComplied()
    {
        var htmlTagsListener = listeners.Get<HtmlTagsListener>();
        var cssTagsListener = listeners.Get<CssTagsListener>();

        var checker = new CssCompliantChecker(
            htmlTagsListener.TraversedTags, 
            cssTagsListener.CssCompliantTags);
        
        return checker.CheckLength() 
            && checker.CheckNames() 
            && checker.CheckAttributes();
    }
}