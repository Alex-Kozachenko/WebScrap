using WebScrap.Tools.Css;
using System.Collections.Immutable;

namespace WebScrap.Processors.CssProcessorListeners;

internal class ProcessedTagsListener(ReadOnlySpan<char> css) : ListenerBase
{
    private readonly ImmutableArray<CssToken> expectedTags
        = CssTokenizer.TokenizeCss(css);
    private readonly Stack<string> processedTags = new();

    public Stack<string> ProcessedTags => new (processedTags.Reverse());

    internal bool IsCssTagMet(ReadOnlySpan<char> tagName)
    {
        var processedTagsCount = processedTags.Count;
        var index = processedTagsCount switch
        {
            < 0 => throw new ArgumentOutOfRangeException(
                $"{nameof(processedTagsCount)} = {processedTagsCount}"),
            var i when i < expectedTags.Length
               => i,
            _ => expectedTags.Length - 1,
        };

        var cssTag = expectedTags[index];
        return tagName.StartsWith(cssTag.Css.Span);
    }
    internal bool IsCompletedCssMet()
        => processedTags.Count == expectedTags.Length;

    protected override void ProcessOpeningTag(ReadOnlySpan<char> tagName)
    {
        if (IsCssTagMet(tagName))
        {
            processedTags.Push(tagName.ToString());
        }
    }

    protected override void ProcessClosingTag(ReadOnlySpan<char> tagName)
    {
        if (IsCssTagMet(tagName))
        {
            processedTags.Pop();
        }
    }
}