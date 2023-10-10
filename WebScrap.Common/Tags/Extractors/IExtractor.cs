namespace WebScrap.Common.Tags.Extractors;

public interface IExtractor<TValue>
{
    ReadOnlySpan<char> Extract(ReadOnlySpan<char> tagContent, out TValue result);
}