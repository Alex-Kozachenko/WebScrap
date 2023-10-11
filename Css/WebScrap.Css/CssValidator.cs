namespace WebScrap.Css;

public static class CssValidator
{
    public static void ThrowIfUnsupported(
        ReadOnlySpan<char> css, 
        ReadOnlySpan<char> bannedChars)
    {
        _ = css.IndexOfAny(bannedChars) switch 
        {
            -1 => 0,
            var i => throw new ArgumentException($"Unable to process css. Css contains unsupported chars: {css[i..]}.")
        };
    }
}