using Core.Tools;
using static Core.Tools.TagNavigator;

namespace Core;

public class HtmlStreamReader
{
    public static ReadOnlySpan<char> Read(
        ReadOnlySpan<char> html, 
        ReadOnlyMemory<char> css)
    {
        var cssTokens = CssTokenizer.Default.TokenizeCss(css);
        html = GoToDeepestTag(html, cssTokens);
        return GetNextInnerText(html);
    }
}