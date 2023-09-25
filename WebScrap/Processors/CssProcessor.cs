using WebScrap.Processors.Common;
using WebScrap.Tools.Html;
using System.Collections.Immutable;
using WebScrap.Processors.CssProcessorListeners;

namespace WebScrap.Processors;

/// <summary>
/// Processes the html with provided css-like-selectors,
/// and returns detected html which conforms 
/// the css from the parameter.
/// </summary>
public class CssProcessor(
    ReadOnlySpan<char> css,
    int htmlLength)
    : ProcessorBase
{
    private readonly List<int> tagIndexes = [];
    private readonly ListenerBase[] listeners = [
        new CssTagsListener(css),
        new HtmlTagsListener(),
    ];
    protected override bool IsDone => Processed >= htmlLength;

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

    protected override void ProcessOpeningTag(
        ReadOnlySpan<char> html,
        ReadOnlySpan<char> tagName)
    {
        if (IsSelfClosingTag(tagName))
        {
            return;
        }

        listeners.ProcessOpeningTag(tagName);
        TryProcessCompletedCss(tagName);
    }

    protected override void ProcessClosingTag(
        ReadOnlySpan<char> html,
        ReadOnlySpan<char> tagName)
    {
        listeners.ProcessClosingTag(tagName);
    }

    private void TryProcessCompletedCss(ReadOnlySpan<char> tagName)
    {
        var cssTagsListener = listeners.Get<CssTagsListener>();
        var htmlTagsListener = listeners.Get<HtmlTagsListener>();

        if (cssTagsListener.IsCssTagMet(tagName) is false)
        {
            return;
        }

        if (cssTagsListener.IsCompletedCssMet() is false)
        { 
            return; 
        }

        if (IsCssComplied(htmlTagsListener, cssTagsListener))
        {
            tagIndexes.Add(Processed);
        }
    }

    private static bool IsCssComplied(
        HtmlTagsListener htmlTagsListener,
        CssTagsListener cssTagsListener)
    {
        var traversedTags = htmlTagsListener.TraversedTags;
        var cssCompliantTags = cssTagsListener.CssCompliantTags;

        if (cssCompliantTags.Count > traversedTags.Count)
        {
            return false;
        }

        while (cssCompliantTags.Count != 0)
        {
            if (!cssCompliantTags.Pop().SequenceEqual(traversedTags.Pop()))
            {
                return false;
            }
        }

        return true;
    }

    private static bool IsSelfClosingTag(ReadOnlySpan<char> tagName)
        => tagName.SequenceEqual("br"); // HACK: unable to calculate < /> right now.
}