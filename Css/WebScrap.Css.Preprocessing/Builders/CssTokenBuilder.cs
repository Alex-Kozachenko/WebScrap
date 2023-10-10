using WebScrap.Css.Common;
using WebScrap.Css.Common.Attributes;
using WebScrap.Css.Common.Selectors;
using WebScrap.Css.Common.Tags;
using WebScrap.Css.Preprocessing.Readers;

namespace WebScrap.Css.Preprocessing;

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
        processed += CssAttributesReader.Read(cssToken[..^processed], out result);
        return this;
    }

    private CssTokenBuilder ReadTag(out CssTagBase result)
    {
        processed += CssTagReader.Read(cssToken[..^processed], out result);
        return this;
    }

    private CssTokenBuilder ReadSelector(out CssSelector result)
    {
        processed += CssSelectorReader.Read(cssToken[..^processed], out result);
        return this;
    }
}