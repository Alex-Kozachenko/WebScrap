using Core.Common;
using Core.Css;
using System.Collections.Immutable;
using static Core.Css.CssTokenizer;

namespace Core;

internal class CssProcessor(ReadOnlySpan<char> css) : IProcessor
{
    private readonly Memory<char> lastTagName = new char[10];
    private int tagsMetCounter = 0;
    private readonly ImmutableArray<CssToken> cssTokens = TokenizeCss(css);

    public bool IsCssCompleted => tagsMetCounter == cssTokens.Length;

    public void ProcessOpeningTag(ReadOnlySpan<char> tagName)
    {
        if (IsCssTagMet(tagName))
        {
            SetLastTagName(tagName);
            tagsMetCounter++;
        }        
    }

    public void ProcessClosingTag(ReadOnlySpan<char> tagName)
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