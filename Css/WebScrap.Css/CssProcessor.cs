using System.Collections.Immutable;
using WebScrap.Common.Tags;
using WebScrap.Common.Tags.Creators;
using WebScrap.Common.Processors;
using WebScrap.Common.Tools;
using WebScrap.Css.Listeners;

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
        TagFactoryBase tagFactory,
        ReadOnlySpan<char> css,
        int htmlLength)
        : base(tagFactory)
    {
        var tagsListeners = new
        {
            css = new CssTagsListener(css),
            tags = new HtmlTagsListener()
        };
        tagsListeners.css.Completed += OnCompletedCssMet;
        // HACK: the order matters, since css has the event.
        listeners = [tagsListeners.tags, tagsListeners.css];

        this.htmlLength = htmlLength;
    }

    public static ImmutableArray<int> CalculateTagIndexes(
        TagFactoryBase tagFactory, 
        ReadOnlySpan<char> html,
        ReadOnlySpan<char> css)
    {
        var processor = new CssProcessor(tagFactory, css, html.Length);
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