namespace Core.Tools;

public enum CssDescendanceKind
{
    Deep = ' ', // main div
    Child = '>', // main>div
}

public record class CssToken(string Css);
public record class ChildCssToken(string Css, CssDescendanceKind kind) 
    : CssToken(Css);

internal class CssTokenizer
{
    public static CssToken[]? TokenizeCss(string css)
    {
        var result = new List<CssToken>();
        var iBase = 0;
        var separators = new[] { ' ', '>' };
        for (int i = 0; i < css.Length; i++)
        {
            if (separators.Contains(css[i]))
            {
                var len = i - iBase;
                result.Add(Extract(css, iBase, len));
                iBase = i;
            }
        }

        result.Add(Extract(css, iBase, css.Length - iBase));
        return [.. result];
    }

    private static CssToken Extract(string css, int iBase, int length)
    {
        var part = css.Substring(iBase, length);
        return TryExtractChildToken(part) 
            ?? new CssToken(part);
    }

    private static ChildCssToken? TryExtractChildToken(string part)
    {
        var descendanceChar = (CssDescendanceKind)part[0];
        if (Enum.IsDefined(descendanceChar))
        {
            var token = part[1..];
            return new ChildCssToken(token, descendanceChar);
        }

        return null;
    }
}