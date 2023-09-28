using WebScrap.Css.Tools;
using System.Collections.Immutable;
using WebScrap.Tools.Html;

namespace WebScrap.Css.Listeners;

/// <summary>
/// Tracs specific CSS-like query.
/// </summary>
internal class CssTagsListener(ReadOnlySpan<char> css) : ListenerBase
{
    private readonly ImmutableArray<CssToken> expectedTags
        = CssTokenizer.TokenizeCss(css);
    private readonly Stack<string> cssTags = new();

    public Stack<string> CssCompliantTags => new (cssTags.Reverse());

    internal bool IsCssTagMet(
        ReadOnlySpan<char> html,
        ReadOnlySpan<char> tag)
    {
        var css = GetCurrentCssToken();
        var entireTag = html[html.IndexOf('>')];
        //
        var attr = css.Attributes.FirstOrDefault();
        var attrDetected = true;

        _ = attr switch
        {
            IdCssAttribute => 1,
            ClassCssAttribute => 1,
            _ => 0
        };
        
        return tag.StartsWith(css.Tag.Span)
            && attrDetected;
    }

    private string? GetAttributeText(
        ReadOnlySpan<char> html, 
        ReadOnlySpan<char> attributeName)
    {
        if (html.Contains(attributeName, StringComparison.InvariantCultureIgnoreCase))
        {
            var index = html.IndexOf(attributeName);
            return html[(index + 1 + 1)..html.IndexOf("\"")].ToString();
        }

        return null;
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