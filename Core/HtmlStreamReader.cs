namespace Core;

public enum CssDescendanceKind
{
    Deep = ' ', // main div
    Child = '>', // main>div
}

public record struct CssToken(
    string Element,
    CssDescendanceKind? DescendanceKind = null);

public class HtmlStreamReader
{
    public int Read(string html, string css)
    {
        var tokenized = TokenizeCss(css);
        return 0;
    }

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
        var desc = (CssDescendanceKind)part[0];
        if (Enum.IsDefined(desc))
        {
            var token = part[1..];
            return new(token, desc);
        }
        else
        {
            return new(part);
        }        
    }
}