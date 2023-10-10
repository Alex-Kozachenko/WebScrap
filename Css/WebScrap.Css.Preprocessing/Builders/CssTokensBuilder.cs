using WebScrap.Css.Common;

namespace WebScrap.Css.Preprocessing.Builders;

internal static class CssTokensBuilder
{
    internal static CssToken[] Build(ReadOnlySpan<char> css)
    {
        css = css.Trim(' ');
        var result = new List<CssToken>();

        while (css.IsEmpty is false)
        {
            var cssToken = GetCssToken(css);
            result.Add(CssTokenBuilder.Build(cssToken));
            css = css[..^cssToken.Length];
        }

        result.Reverse();
        return [.. result];
    }

    private static ReadOnlySpan<char> GetCssToken(ReadOnlySpan<char> css) 
        => css.LastIndexOfAny([' ', '>']) switch
        {
            -1 => css,
            var i => css[i..]
        };
}