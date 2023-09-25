using WebScrap.Processors.Common;
using WebScrap.Tools.Css;
using WebScrap.Tools.Html;
using System.Collections.Immutable;

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
    private readonly ImmutableArray<CssToken> expectedTags
        = CssTokenizer.TokenizeCss(css);
    private readonly List<int> tagIndexes = [];
    private readonly Stack<string> traversedTags = new();
    private readonly Stack<string> processedTags = new();
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

        traversedTags.Push(tagName.ToString());
        if (IsCssTagMet(tagName))
        {
            processedTags.Push(tagName.ToString());
            TryProcessCompletedCss();
        }
    }

    protected override void ProcessClosingTag(
        ReadOnlySpan<char> html,
        ReadOnlySpan<char> tagName)
    {
        if (IsCssTagMet(tagName))
        {
            processedTags.Pop();
        }
        traversedTags.Pop();
    }

    private void TryProcessCompletedCss()
    {
        if (processedTags.Count == expectedTags.Length)
        {
            if (EndsWith(traversedTags, processedTags))
            {
                tagIndexes.Add(Processed);
            }
        }
    }

    private bool EndsWith(Stack<string> bigStack, Stack<string> subStack)
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

    private bool IsCssTagMet(ReadOnlySpan<char> tagName)
    {
        // TODO: extract concerns. Too many specific fields.
        var lastProcessedTagIndex = processedTags.Count;
        var index = lastProcessedTagIndex switch
        {
            < 0 => throw new ArgumentOutOfRangeException(
                $"{nameof(lastProcessedTagIndex)} = {lastProcessedTagIndex}"),
            var i when i < expectedTags.Length
               => i,
            _ => expectedTags.Length - 1,
        };

        var cssTag = expectedTags[index];
        return tagName.StartsWith(cssTag.Css.Span);
    }

    private static bool IsSelfClosingTag(ReadOnlySpan<char> tagName)
        => tagName.SequenceEqual("br"); // HACK: unable to calculate < /> right now.
}