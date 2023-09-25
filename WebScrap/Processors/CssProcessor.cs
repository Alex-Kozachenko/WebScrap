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
    private int tagsMetCounter = 0;
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
        if (!IsCssTagMet(tagName))
        {
            return;
        }
        tagsMetCounter++;
        TryProcessCompletedCss();
        
    }

    protected override void ProcessClosingTag(
        ReadOnlySpan<char> html,
        ReadOnlySpan<char> tagName)
    {
        if (IsCssTagMet(tagName))
        {
            tagsMetCounter--;
        }
    }

    private void TryProcessCompletedCss()
    {
        if (tagsMetCounter == expectedTags.Length)
        {
            tagIndexes.Add(Processed);
        }
    }

    private bool IsCssTagMet(ReadOnlySpan<char> tagName)
    {
        var index = tagsMetCounter switch
        {
            < 0 => throw new ArgumentOutOfRangeException(
                $"{nameof(tagsMetCounter)} = {tagsMetCounter}"),
            var i when i < expectedTags.Length
               => i,
            _ => expectedTags.Length - 1,
        };

        var cssTag = expectedTags[index];
        return tagName.StartsWith(cssTag.Css.Span);
    }
}