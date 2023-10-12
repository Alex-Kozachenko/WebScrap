namespace WebScrap.Core.Tags.Extractors;

internal interface IExtractor<TValue>
{
    ReadOnlySpan<char> Extract(ReadOnlySpan<char> tagContent, out TValue result);
}