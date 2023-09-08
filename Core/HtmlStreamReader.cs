using Core.Tools;

namespace Core;

public class HtmlStreamReader
{
    public string Read(ReadOnlySpan<char> htmlSpan, string css)
    {
        var cssTokens = CssTokenizer.Default.TokenizeCss(css);

        ReadOnlySpan<char> tagValueSpan = null;
        while (cssTokens.Count is not 0)
        {
            var peek = cssTokens.Peek().Css.Span;
            var beginTagIndex = htmlSpan.IndexOf('<') + 1;
            tagValueSpan = htmlSpan.Slice(
                beginTagIndex, 
                peek.Length);

            if (tagValueSpan.SequenceEqual(peek) is not true)
            {
                return string.Empty;
            }
            htmlSpan = htmlSpan[(beginTagIndex+peek.Length)..];
            cssTokens.Dequeue();
        }
        
        return tagValueSpan.ToString();
    }
}