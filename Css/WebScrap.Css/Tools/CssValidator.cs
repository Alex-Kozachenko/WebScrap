namespace WebScrap.Css;

/// <summary>
/// Represents a set of rules for css entries.
/// </summary>
public static class CssValidator
{
    /// <summary>
    /// Checks if css contains unsupported attributes selector.
    /// </summary>
    public static void ThrowIfUnsupportedAttribute(ReadOnlySpan<char> css)
        => ThrowIfUnsupported(css, "[]");

    /// <summary>
    /// Checks if css contains unsupported selectors.
    /// </summary>
    public static void ThrowIfUnsupportedCharacters(ReadOnlySpan<char> css)
        => ThrowIfUnsupported(css, ",+");

    private static void ThrowIfUnsupported(
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