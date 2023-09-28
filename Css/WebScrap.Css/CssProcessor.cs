using WebScrap.Processors.Common;
using WebScrap.Tools.Html;
using System.Collections.Immutable;
using WebScrap.Css.Listeners;
using WebScrap.Tags;
 
namespace WebScrap.Css;

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

    protected override void Process(
        ReadOnlySpan<char> html,
        OpeningTag tag)
    {
        listeners.Process(tag);
        TryProcessCompletedCss(html,tag);
    }

    protected override void Process(
        ReadOnlySpan<char> html,
        ClosingTag tag)
    {
        listeners.Process(tag);
    }

    private void TryProcessCompletedCss(
        ReadOnlySpan<char> html,
        OpeningTag tag)
    {
        var cssTagsListener = listeners.Get<CssTagsListener>();
        var htmlTagsListener = listeners.Get<HtmlTagsListener>();

        if (cssTagsListener.IsCssTagMet(tag) is false)
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
            var traversedTag = traversedTags.Pop();
            var compliantTag = cssCompliantTags.Pop();
            if (!compliantTag.Name.SequenceEqual(traversedTag.Name))
            {
                return false;
            }

            if (traversedTag.Attributes.Count == compliantTag.Attributes.Count)
            {
                for (int i = 0; i < traversedTag.Attributes.Count; i++)
                {
                    var tAttr = traversedTag.Attributes;
                    var cAttr = compliantTag.Attributes;

                    if (!cAttr.SequenceEqual(tAttr))
                    {
                        return false;
                    }
                }
            }
        }

        return true;
    }
}