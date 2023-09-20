using Core.Css;
using Core.Html.Reading.Tags;
using Core.Html.Tools;
using System.Collections.Immutable;

namespace Core.Processors;

internal class CssProcessor(ReadOnlySpan<char> css, int htmlLength) : ProcessorBase
{
    private readonly ImmutableArray<CssToken> cssTokens = CssTokenizer.TokenizeCss(css);
    private readonly int htmlLength = htmlLength;
    private List<Range> ranges = new();
    private readonly Memory<char> lastTagName = new char[10];
    private int tagsMetCounter = 0;
    
    public ImmutableArray<Range> Ranges => ranges.ToImmutableArray();
    public override bool IsDone => Processed >= htmlLength;

    public override int Prepare(ReadOnlySpan<char> html) => 0;

    public override int Proceed(ReadOnlySpan<char> html)
        => TagsNavigator.GetNextTagIndex(html[1..]) + 1;

    protected override void ProcessOpeningTag(
        ReadOnlySpan<char> html, 
        ReadOnlySpan<char> tagName)
    {
        if (IsCssTagMet(tagName))
        {
            SetLastTagName(tagName);
            tagsMetCounter++;
        }

        var isCssCompleted = tagsMetCounter == cssTokens.Length;
        if (isCssCompleted)
        {
            var tagLength = HtmlTagExtractor.GetEntireTagLength(html);
            var bodyRange = Processed..(Processed + tagLength);
            ranges.Add(bodyRange);
        }
    }

    protected override void ProcessClosingTag(
        ReadOnlySpan<char> html,
        ReadOnlySpan<char> tagName)
    {
        if (IsCssTagMet(tagName) 
            && IsLastTagEqualTo(tagName))
        {
            tagsMetCounter--;
        }        
    }

    private bool IsLastTagEqualTo(ReadOnlySpan<char> tagName)
    {
        var trimmed = lastTagName.TrimEnd((char)0);
        return trimmed.Span.SequenceEqual(tagName);
    }

    private bool IsCssTagMet(ReadOnlySpan<char> tagName)
    {
        var cssTag = tagsMetCounter < cssTokens.Length
            ? cssTokens[tagsMetCounter]
            : new CssToken();

        return tagName.StartsWith(cssTag.Css.Span);
    }

    private void SetLastTagName(ReadOnlySpan<char> tagName)
    {
        lastTagName.Span.Clear();
        tagName.CopyTo(lastTagName.Span);
    }
}