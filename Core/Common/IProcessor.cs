namespace Core.Common;

internal interface IProcessor
{
    public void ProcessOpeningTag(ReadOnlySpan<char> tagName);
    public void ProcessClosingTag(ReadOnlySpan<char> tagName);
}