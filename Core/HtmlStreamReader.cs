using static Core.Internal.HtmlProcessing.TagNavigator;
using static Core.Internal.HtmlProcessing.TextExtractor;

namespace Core;

public class HtmlStreamReader
{
    public static ReadOnlySpan<char> Read(
        ReadOnlySpan<char> html, 
        ReadOnlyMemory<char> css)
    {
        var cssTokens = CssTokenizer.Default.TokenizeCss(css);
        html = html.GoToTagByCss(cssTokens);
        return Extract(html);
    }
}