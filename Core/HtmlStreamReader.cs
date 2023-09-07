using Core.Tools;

namespace Core;

public class HtmlStreamReader
{
    public string Read(ReadOnlySpan<char> htmlSpan, string css)
    {
        var cssTokens = CssTokenizer.Default.TokenizeCss(css);
        // 1. find by tag <main>
        var beginTagIndex = htmlSpan.IndexOf('<') + 1;
        var peekSpan = cssTokens.Peek().Css.Span;
        var tagValueSpan = htmlSpan.Slice(
            beginTagIndex, 
            peekSpan.Length);
        
        if (tagValueSpan.SequenceEqual(peekSpan))
        {
            return tagValueSpan.ToString();
        }
        // 2. find nested tag <p>
        // 3. figure out the revelance
        return string.Empty;
    }
}