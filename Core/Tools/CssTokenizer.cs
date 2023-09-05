namespace Core.Tools;

internal record struct CssToken(char? ChildSelector, string Css);

internal class CssTokenizer
{
    internal static CssToken[] TokenizeCss(string css)
    {
        // TODO: make an ordered enumerable.
        var result = new List<CssToken>();
        var iBase = 0;
        var separators = new[] { ' ', '>' };
        for (int i = 0; i < css.Length; i++)
        {
            if (separators.Contains(css[i]))
            {
                var len = i - iBase;
                result.Add(ExtractCssToken(css, iBase, len));
                iBase = i;
            }
        }

        result.Add(ExtractCssToken(css, iBase, css.Length - iBase));
        return [.. result];
    }

    private static CssToken ExtractCssToken(string css, int iBase, int length)
    {
        Span<char> childSelectors = [' ', '>'];

        return css.Substring(iBase, length) switch
        {
            var token when childSelectors.Contains(token.First())
                => new(token[0], token[1..]),
            var token => new(null, token)
        };
    }        
}