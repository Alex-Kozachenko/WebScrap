namespace WebScrap.Tags.Tools;

internal static class AttributeExtractor
{
    internal static ILookup<string, string> EmptyAttributes 
        => Array.Empty<int>().ToLookup(x => "", x => "");

    internal static ILookup<string, string> GetAttributes(
        ReadOnlySpan<char> tagContent)
    {
        tagContent = tagContent.TrimEnd('/');
        var key = GetKey(tagContent).ToString();
        var values =  GetValues(tagContent); 

        return values?.ToLookup(x => key, x => x)
            ?? EmptyAttributes;   
    }

    private static ReadOnlySpan<char> GetKey(ReadOnlySpan<char> tagContent) 
        => tagContent.IndexOf('=') switch
        {
            -1 => tagContent,
            var index => tagContent[..index]
        };

    private static string[]? GetValues(ReadOnlySpan<char> tagContent)
        => tagContent.IndexOf('=') switch
        {
            -1 => null,
            var index => GetValues(tagContent, index)
        };

    private static string[] GetValues(
        ReadOnlySpan<char> tagContent,
        int index)
    {
        const string valueScreen = "\"'";

        index++;
        tagContent = tagContent[index..]
            .Clip(valueScreen, valueScreen);

        var ranges = GetRanges(tagContent)
            .Where(x => x.End.Value != 0);

        return [..GetStrings(tagContent, ranges)];
    }

    private static Range[] GetRanges(
        ReadOnlySpan<char> tagContent)
    {
        const int maxAttributeValuesCount = 100;
        Span<Range> ranges = stackalloc Range[maxAttributeValuesCount];
        return tagContent.Split(ranges, ' ') switch
        {
            0 => [0..tagContent.Length],
            _ => [..ranges]
        };
    }

    private static string[] GetStrings(
        ReadOnlySpan<char> tagContent, 
        IEnumerable<Range> ranges)
    {
        var result = new List<string>();        
        foreach (var range in ranges)
        {
            result.Add(tagContent[range].ToString());
        }
        return [.. result];
    }
}