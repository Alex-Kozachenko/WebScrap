namespace WebScrap.Core.Tags.Tools;

internal class CommentsSkipper 
{
    internal static bool TrySkipComment(ReadOnlySpan<char> html, out int processed)
    {
        const string comment = "<!--";

        processed = html.StartsWith(comment) 
            ? comment.Length + SkipComment(html[comment.Length..])
            : 0;

        return processed != 0;
    }

    static int SkipComment(ReadOnlySpan<char> html)
    {
        return (html.IndexOf('-'), html.IndexOf('>')) switch
        {
            (0, 1) => TagsNavigator.GetNextTagIndex(html), // HACK: it actually skips usefull chars right after the comment. It should return "->".Length. It's the design issue, unfortunately.
            (0, _) => 1 + SkipComment(html[1..]),
            (var x, _) => x + SkipComment(html[x..])
        };
    }
}