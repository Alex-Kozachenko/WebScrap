using System.Text.RegularExpressions;
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
            // I am breaking the tags here.
            // TODO: make indexes?
            htmlSpan = htmlSpan[(beginTagIndex+peek.Length)..];
            cssTokens.Dequeue();
        }

        htmlSpan = htmlSpan[1..];
        var enclosingTag = $"</{tagValueSpan}>".AsSpan(); // HACK: form a span instead of a string, please.

        // find next </p>
        // var endTagIndex = htmlSpan.IndexOf('</p>');
        var regex = new Regex($".*</{tagValueSpan}>");
        var match = regex.Match(htmlSpan.ToString());
        
        // is that a joke?
        return match.Value[..($"</{tagValueSpan}>".Length+1)];
    }
}