namespace WebScrap.Core.Tags.Extracting;

internal interface IExtractor<TValue>
{
    ReadOnlySpan<char> Extract(ReadOnlySpan<char> tagContent, out TValue result);
}