namespace WebScrap.Tags.Attributes;

public interface IExtractor<TValue>
{
    ReadOnlySpan<char> Extract(ReadOnlySpan<char> tagContent, out TValue result);
}