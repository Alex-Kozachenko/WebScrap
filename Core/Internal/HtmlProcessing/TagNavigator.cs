namespace Core.Internal.HtmlProcessing;

internal static class TagNavigator
{
    internal static ReadOnlySpan<char> GoToTagByCss(
            this ReadOnlySpan<char> html,
            Queue<CssToken> cssTokens)
    {
        var cssLine = string.Concat(cssTokens.Select(x => x.ToString()));
        html = GoToNextTag(html);
        while (true)
        {
            var tag = cssTokens.Dequeue().Css.Span;            
            AssertCurrentTag(html[1..], tag, cssLine);
            if (cssTokens.Count is 0)
            {
                return html;
            }
            html = GoToNextTag(html);
        }
    }

    internal static ReadOnlySpan<char> GoToNextTag(this ReadOnlySpan<char> html)
    {
        const int tagCharOffset = 1; // in case we are standing on <
        var nextTagIndex = html[tagCharOffset..].IndexOf('<');
        return nextTagIndex switch
        {
            -1 => html[..^1],
            _ => html[tagCharOffset..][nextTagIndex..],
        };
    }

    internal static ReadOnlySpan<char> GoToTagBody(this ReadOnlySpan<char> html)
    {
        // HACK: the design went really dirty so i am patching up like this. Just wanna see its working.
        if (html.IsEmpty)
        {
            return html;
        }


        var (openingTagIndex, closingTagIndex) = (html.IndexOf('<'), html.IndexOf('>'));

        if (openingTagIndex is not 0)
        {
            // NOTE: forces the user to read current tag body.
            return html; 
        }

        return closingTagIndex switch
        {
            -1 => html[..^1],
            _ => html[(closingTagIndex + 1)..],
        };
    }

    private static void AssertCurrentTag(
        this ReadOnlySpan<char> html,
        ReadOnlySpan<char> currentTag,
        ReadOnlySpan<char> cssQuery)
    {
        if (html.StartsWith(currentTag) is not true)
        {
            throw new InvalidOperationException(
                $"""
                    Unable to locate a tag:"{currentTag}"
                    under for css: "{cssQuery}"
                    html: {html}
                """);
        }
    }   
}