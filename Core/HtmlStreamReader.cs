namespace Core;

public class HtmlStreamReader
{
    public int Read(string html, string css)
    {
        var tokenized = TokenizeCss(css);
        return 0;
    }

    public static string[]? TokenizeCss(string css)
    {
        var result = new List<string>();
        var iBase = 0;
        var separators = new[] { ' ', '>' };
        for (int i = 0; i < css.Length; i++)
        {
            if (separators.Contains(css[i]))
            {
                var len = i - iBase;
                result.Add(css.Substring(iBase, len));
                iBase = i;
            }
        }
        result.Add(css.Substring(iBase));
        return [.. result];
    }
}