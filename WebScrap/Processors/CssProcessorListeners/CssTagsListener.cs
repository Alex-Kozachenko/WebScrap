using WebScrap.Tools.Css;
using System.Collections.Immutable;

namespace WebScrap.Processors.CssProcessorListeners;

/// <summary>
/// Tracs specific CSS-like query.
/// </summary>
internal class CssTagsListener(ReadOnlySpan<char> css) : ListenerBase
{
    private readonly ImmutableArray<CssToken> expectedTags
        = CssTokenizer.TokenizeCss(css);
    private readonly Stack<string> cssTags = new();

    public Stack<string> CssCompliantTags => new (cssTags.Reverse());

    internal bool IsCssTagMet(ReadOnlySpan<char> tag)
    {
        var processedTagsCount = cssTags.Count;
        var index = processedTagsCount switch
        {
            < 0 => throw new ArgumentOutOfRangeException(
                $"{nameof(processedTagsCount)} = {processedTagsCount}"),
            var i when i < expectedTags.Length
               => i,
            _ => expectedTags.Length - 1,
        };

        var css = expectedTags[index];
        return tag.StartsWith(css.Tag.Span);
    }
    internal bool IsCompletedCssMet()
        => cssTags.Count == expectedTags.Length;

    internal override void ProcessOpeningTag(ReadOnlySpan<char> tagName)
    {
        if (IsCssTagMet(tagName))
        {
            cssTags.Push(tagName.ToString());
        }
    }

    internal override void ProcessClosingTag(ReadOnlySpan<char> tagName)
    {
        if (IsCssTagMet(tagName))
        {
            cssTags.Pop();
        }
    }
}