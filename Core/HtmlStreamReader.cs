using Core.Tools;

namespace Core;

public class HtmlStreamReader
{
    public string Read(ReadOnlySpan<char> htmlSpan, string css)
    {
        htmlSpan = MoveToInnerTag(htmlSpan, css);
        var innerRange = (htmlSpan.IndexOf('>') + 1)..htmlSpan.IndexOf('<');
        return htmlSpan[innerRange].ToString();
    }

    private static bool CheckHtmlStartsWithTag(ReadOnlySpan<char> html,
            ReadOnlySpan<char> tag)
    {
        var beginTagIndex = html.IndexOf('<');
        var currentTagFromHtml = html.Slice(
            beginTagIndex + 1, 
            tag.Length);

        return currentTagFromHtml.SequenceEqual(tag);
    }
    
    private static ReadOnlySpan<char> MoveToInnerTag(
            ReadOnlySpan<char> htmlSpan, 
            string css)
    {
        var cssTokens = CssTokenizer.Default.TokenizeCss(css);
        while (cssTokens.Count is not 0)
        {
            var currentCssTag = cssTokens.Dequeue().Css.Span;
            // NOTE: structure critical. Please fix to flexible.
            if (!CheckHtmlStartsWithTag(htmlSpan, currentCssTag))
            {
                return string.Empty;
            }

            var beginTagIndex = htmlSpan.IndexOf('<');
            htmlSpan = htmlSpan[(beginTagIndex + 1)..]; // skip opening bracket <
        }

        return htmlSpan;
    }
}