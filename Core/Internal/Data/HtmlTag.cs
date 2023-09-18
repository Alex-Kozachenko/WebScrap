namespace Core.Internal.Data;

internal readonly ref struct HtmlTag(ReadOnlySpan<char> name, bool isOpening)
{
    public ReadOnlySpan<char> Name { get; } = name;
    public bool IsOpening { get; } = isOpening;
}