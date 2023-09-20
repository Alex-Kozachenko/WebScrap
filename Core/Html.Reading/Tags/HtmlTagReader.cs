namespace Core.Html.Reading.Tags;

public enum HtmlTagKind
{
    Opening,
    Closing
}

/// <summary>
/// Produces shallow <see cref="HtmlTag"/>= object.
/// </summary>
internal class HtmlTagReader
{
    internal static HtmlTagKind GetHtmlTagKind(
        ReadOnlySpan<char> html)
    {
        return (html[0], html[1]) switch
        {
            ('<', '/') => HtmlTagKind.Closing,
            ('<', _) => HtmlTagKind.Opening,
            _ => throw new ArgumentException($"Html doesnt start with tag. {html}")
        };
    }
}