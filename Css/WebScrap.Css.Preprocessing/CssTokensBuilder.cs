using WebScrap.Css.Common;
using WebScrap.Css.Preprocessing.Readers;

namespace WebScrap.Css.Preprocessing;

public static class CssTokensBuilder
{
    private static readonly char[] childSelectors = [' ', '>'];

    public static CssToken[] Build(ReadOnlySpan<char> css)
    {
        css = css.Trim(' ');
        var processed = 0;
        var result = new List<CssToken>();

        while (css.IsEmpty is false)
        {
            processed = CssAttributesReader.Read(css, out var attributes);
            css = css[..^processed];

            processed = CssTagReader.Read(css, out var tag);
            css = css[..^processed];

            char? childSelector = css.IsEmpty ? null : css[^1];
            processed = CssSelectorReader.Read(childSelector, out var selector);
            css = css[..^processed];

            result.Add(new CssToken(selector, tag, attributes));
        }

        result.Reverse();
        return [.. result];
    }
}