using Core.Internal.HtmlProcessing;
using static Core.Internal.HtmlProcessing.TagNavigator;
using static Core.Internal.HtmlProcessing.TextExtractor;

namespace Core;

public class HtmlStreamReader
{
    public static ReadOnlySpan<char> Read(
        ReadOnlySpan<char> html, 
        ReadOnlyMemory<char> css)
    {
        return html
            .GoToTagByCss(css)
            .ReadBody();
    }
}