using System.Collections.Frozen;

namespace Core.Tools;

internal record struct CssToken(char? ChildSelector, string Css);

internal readonly ref struct CssTokenizer(Span<char> tokenDelimeters)
{
    private readonly Span<char> tokenDelimeters = tokenDelimeters;

    internal static CssTokenizer Default
        => new([' ', '>']);

    internal FrozenSet<CssToken> TokenizeCss(string css)
    {
        var result = new Queue<CssToken>();
        var iBase = 0;

        for (int i = 0; i < css.Length; i++)
        {
            if (tokenDelimeters.Contains(css[i]))
            {
                var len = i - iBase;
                var token = ExtractCssToken(css, iBase, len);
                result.Enqueue(token);
                iBase = i;
            }
        }

        var finalToken = ExtractCssToken(css, iBase, css.Length - iBase);
        result.Enqueue(finalToken);
        return result.ToFrozenSet();
    }

    private CssToken ExtractCssToken(string css, int iBase, int length)
    {
        return css.Substring(iBase, length) switch
        {
            var token when tokenDelimeters.Contains(token.First())
                => new(token[0], token[1..]),
            var token => new(null, token)
        };
    }        
}