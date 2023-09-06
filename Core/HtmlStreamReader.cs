using Core.Tools;

namespace Core;

public class HtmlStreamReader
{
    public int Read(string html, string css)
    {
        var tokenized = CssTokenizer.Default.TokenizeCss(css);
        return 0;
    }
}