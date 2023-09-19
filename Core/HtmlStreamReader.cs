using Core.Internal.HtmlProcessing;
using static Core.Internal.HtmlProcessing.TagsLocator;
using static Core.Internal.HtmlProcessing.Extractors.HtmlProcessor;

namespace Core;

public class HtmlStreamReader
{
    public static ReadOnlySpan<char> Read(
        ReadOnlySpan<char> html, 
        ReadOnlySpan<char> css)
    {
        var tag =  LocateTagsByCss(html, css);
        throw new NotImplementedException();
        // var body = tag.ReadBody();
        // return body;
    }
}