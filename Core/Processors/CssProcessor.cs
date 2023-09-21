using Core.Css;
using Core.Html.Tools;
using System.Collections.Immutable;
using static Core.Processors.TagsProcessor;

namespace Core.Processors;

internal class CssProcessor(
    ReadOnlySpan<char> css,
    int htmlLength)
    : ProcessorBase
{
    private readonly ImmutableArray<CssToken> expectedTags 
        = CssTokenizer.TokenizeCss(css);
    private readonly List<Range> ranges = new();
    private int tagsMetCounter = 0;
    protected override bool IsDone => Processed >= htmlLength;

    public static ImmutableArray<Range> CalculateRanges(
        ReadOnlySpan<char> html,
        ReadOnlySpan<char> css)
    {
        html = HtmlValidator.ToValidHtml(html);
        var processor = new CssProcessor(css, html.Length);
        processor.Run(html);
        return [.. processor.ranges];
    }

    protected override int Prepare(ReadOnlySpan<char> html) => 0;

    protected override int Proceed(ReadOnlySpan<char> html)
        => TagsNavigator.GetNextTagIndex(html[1..]) + 1;

    protected override void ProcessOpeningTag(
        ReadOnlySpan<char> html,
        ReadOnlySpan<char> tagName)
    {
        if (IsCssTagMet(tagName))
        {
            tagsMetCounter++;
        }

        var isCssCompleted = tagsMetCounter == expectedTags.Length;
        if (isCssCompleted)
        {
            var tagLength = GetEntireTagLength(html);
            var bodyRange = Processed..(Processed + tagLength);
            ranges.Add(bodyRange);
        }
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

    private bool IsCssTagMet(ReadOnlySpan<char> tagName)
    {
        var index = tagsMetCounter switch 
        {
            <0 => throw new ArgumentOutOfRangeException(
                $"{nameof(tagsMetCounter)} = {tagsMetCounter}"),
            var i when i < expectedTags.Length
               => i,
            _ => expectedTags.Length - 1,
        };

        var cssTag = expectedTags[index];
        return tagName.StartsWith(cssTag.Css.Span);
    }
}