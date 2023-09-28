using WebScrap.Css.Tools;
using System.Collections.Immutable;
using WebScrap.Tools.Html;
using WebScrap.Tags;

namespace WebScrap.Css.Listeners;

/// <summary>
/// Tracs specific CSS-like query.
/// </summary>
internal class CssTagsListener(ReadOnlySpan<char> css) : ListenerBase
{
    private readonly ImmutableArray<CssToken> expectedTags
        = CssTokenizer.TokenizeCss(css);
    private readonly Stack<OpeningTag> cssTags = new();

    public Stack<OpeningTag> CssCompliantTags => new (cssTags.Reverse());

    internal override void Process(OpeningTag tag)
    {
        if (IsCssTagMet(tag))
        {
            cssTags.Push(tag);
        }
    }

    internal override void Process(ClosingTag tag)
    {
        if (IsCssTagMet(tag))
        {
            if (cssTags.Any())
            {
                cssTags.Pop();
            }
        }
    }

    internal bool IsCssTagMet(OpeningTag tag)
    {
        var css = GetCurrentCssToken();
        var attr = css.Attributes.FirstOrDefault();
        string selectorKey = attr switch
        {
            IdCssAttribute => "id",
            ClassCssAttribute => "class",
            _ => ""
        };

        if (tag.Attributes.Count != css.Attributes.Length)
        {
            return false;
        }

        if (tag.Attributes.Count != 0 && css.Attributes.Any())
        {
            var nameMet = tag.Name.AsSpan().SequenceEqual(css.Tag.Span);        
            var firstValue = tag.Attributes[selectorKey].First().AsSpan();
            var attrMet = attr?.SelectorText.Span.SequenceEqual(firstValue) ?? false;
            return attrMet && nameMet;
        }

        return tag.Name.AsSpan().SequenceEqual(css.Tag.Span);
        

    }

    internal bool IsCssTagMet(ClosingTag tag)
    {
        var css = GetCurrentCssToken();
        var nameMet = tag.Name.AsSpan().StartsWith(css.Tag.Span);
        return nameMet;
    }

    internal bool IsCompletedCssMet()
        => cssTags.Count == expectedTags.Length;


    private CssToken GetCurrentCssToken()
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

        return expectedTags[index];
    }
}