namespace WebScrap.Tags.Tools;

internal static class AttributeExtractor
{
    internal static ILookup<string, string?> EmptyAttributes 
        => Array.Empty<int>().ToLookup(x => "", x => (string?)null);

    internal static ILookup<string, string?> GetAttributes(
        ReadOnlySpan<char> tagContent)
    {
        tagContent = tagContent.TrimEnd('/');

        var keyValues = new List<KeyValuePair<string, string?>>();
        while (tagContent.IsEmpty is false)
        {         
            tagContent = tagContent.TrimStart(' ');   
            var key = GetKey(tagContent).ToString();
            var values = GetValues(tagContent);

            if (values is null)
            {
                keyValues.Add(new KeyValuePair<string, string?>(key, null));
            }
            else
            {
                var lookup = values.Select(v => new KeyValuePair<string, string?>(key, v))
                .ToArray();
                keyValues.AddRange(lookup);
            }
                
            var delimeterOffset = values switch 
            {
                null => 0,
                _ => 2,
            };

            var offset = tagContent
                [key.Length..]
                [delimeterOffset..];

            tagContent = values switch 
            {
                
                null => offset,
                _ => offset
                    [offset.IndexOfAny("\"'")..]                
                    [1..]
            };
        }
        
        return keyValues.ToLookup(x => x.Key, x => x.Value);
    }

    private static ReadOnlySpan<char> GetKey(ReadOnlySpan<char> tagContent) 
        => tagContent.IndexOfAny(" =") switch
        {
            -1 => tagContent,
            var index => tagContent[..index]
        };

    private static string[]? GetValues(ReadOnlySpan<char> tagContent)
    {
        var spaceIndex = tagContent.IndexOf(' ') switch 
        {
            -1 => int.MaxValue,
            var i => i
        };

        return (spaceIndex, tagContent.IndexOf('=')) switch
        {
            (var x, var y)
                when x < y
                => null,
            (_, -1) => null,
            (_, var index) => GetValues(tagContent, index)
        };
    }
        

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