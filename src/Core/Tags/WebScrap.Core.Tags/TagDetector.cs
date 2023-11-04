using WebScrap.Core.Tags.Data;

namespace WebScrap.Core.Tags;

internal class TagDetector
{
    internal static TagKind Detect(ReadOnlySpan<char> currentHtml)
    {
        if (!currentHtml.StartsWith("<"))
        {
            throw new ArgumentException($"Html should start with tag. {currentHtml}");
        }

        return currentHtml.Clip("<", ">", true) switch
        {
            var a when a[0] == '!' 
                => TagKind.Comment,
            var a when a[^1] == '/' 
                => TagKind.Inline,
            var a when a[0] == '/' 
                => TagKind.Closing,
            _ => TagKind.Opening
        };
    }
}