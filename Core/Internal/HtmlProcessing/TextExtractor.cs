namespace Core.Internal.HtmlProcessing;

internal static class TextExtractor
{
    internal static string ReadBody(this ReadOnlySpan<char> html)
    {
        const int aMagickNumberWhichForcesToSkipClosingTag = 1;
        var (nextOpeningBracketIndex, nextClosingBracketIndex) =
            (html.IndexOf('<'), html.IndexOf('>'));

        if (nextOpeningBracketIndex is 0)
        {
            return html
                [nextClosingBracketIndex..]
                [aMagickNumberWhichForcesToSkipClosingTag..]
                .ReadBody();
        }
        
        var isCurrentPositionInsideTag = 
            nextOpeningBracketIndex > nextClosingBracketIndex;

        if (isCurrentPositionInsideTag)
        {
            return html
                [nextClosingBracketIndex..]
                [aMagickNumberWhichForcesToSkipClosingTag..]
                .ReadBody();
        }

        return nextOpeningBracketIndex switch
        {
            -1 => new string(html),
            var i => new string(html[..i]) + ReadBody(html[i..]),
        };
    }
}