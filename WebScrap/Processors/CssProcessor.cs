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
        new ProcessedTagsListener(css),
        new TraversedTagsListener(),
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

        ListenerBase.ProcessOpeningTag(listeners, tagName);
        TryProcessCompletedCss(tagName);
    }

    protected override void ProcessClosingTag(
        ReadOnlySpan<char> html,
        ReadOnlySpan<char> tagName)
    {
        ListenerBase.ProcessClosingTag(listeners, tagName);
    }

    private void TryProcessCompletedCss(ReadOnlySpan<char> tagName)
    {
        var processedTagsListener = listeners.OfType<ProcessedTagsListener>().First();
        var traversedTagsListener = listeners.OfType<TraversedTagsListener>().First();

        if (processedTagsListener.IsCssTagMet(tagName) is false)
        {
            return;
        }

        if (processedTagsListener.IsCompletedCssMet() is false)
        { 
            return; 
        }

        if (EndsWith(
            traversedTagsListener.TraversedTags,
            processedTagsListener.ProcessedTags))
        {
            tagIndexes.Add(Processed);
        }
    }

    private static bool EndsWith(Stack<string> bigStack, Stack<string> subStack)
    {
        if (subStack.Count > bigStack.Count)
        {
            return false;
        }

        // HACK: a ton.
        var _bigStack = new Stack<string>(bigStack.Reverse());
        var _subStack = new Stack<string>(subStack.Reverse());
        while (_subStack.Count != 0)
        {
            if (!_subStack.Pop().SequenceEqual(_bigStack.Pop()))
            {
                return false;
            }
        }

        return true;
    }

    private static bool IsSelfClosingTag(ReadOnlySpan<char> tagName)
        => tagName.SequenceEqual("br"); // HACK: unable to calculate < /> right now.
}