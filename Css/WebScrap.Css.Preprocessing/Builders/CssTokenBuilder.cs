using WebScrap.Css.Common;
using WebScrap.Css.Common.Attributes;
using WebScrap.Css.Common.Selectors;
using WebScrap.Css.Common.Tags;
using WebScrap.Css.Preprocessing.Readers;

namespace WebScrap.Css.Preprocessing.Builders;

internal ref struct CssTokenBuilder(ReadOnlySpan<char> cssToken)
{
    private readonly ReadOnlySpan<char> cssToken = cssToken;
    private int processed = 0;

    public static CssToken Build(ReadOnlySpan<char> cssToken)
    {
        new CssTokenBuilder(cssToken)
            .ReadAttributes(out var attributes)
            .ReadTag(out var tag)
            .ReadSelector(out var selector);
        
        return new (selector, tag, attributes);
    }

    private CssTokenBuilder ReadAttributes(out CssAttributesLookup result)
    {
        processed += CssAttributesReader.Read(UnprocessedCssToken, out result);
        return this;
    }

    private CssTokenBuilder ReadTag(out CssTagBase result)
    {
        processed += CssTagReader.Read(UnprocessedCssToken, out result);
        return this;
    }

    private CssTokenBuilder ReadSelector(out CssSelector result)
    {
        processed += CssSelectorReader.Read(UnprocessedCssToken, out result);
        return this;
    }

    private readonly ReadOnlySpan<char> UnprocessedCssToken 
        => cssToken[..^processed];
}