using WebScrap.Css.Contracts;
using WebScrap.Css.Data;

namespace WebScrap.Css.Preprocessing;

public class TokensBuilder : ITokensBuilder
{
    public CssToken[] Build(ReadOnlySpan<char> css)
    {
        css = css.Trim(' ');
        CssValidator.ThrowIfUnsupportedCharacters(css);
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