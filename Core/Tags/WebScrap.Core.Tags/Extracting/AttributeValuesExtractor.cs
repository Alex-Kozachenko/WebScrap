namespace WebScrap.Core.Tags.Extracting;

internal sealed class AttributeValuesExtractor
{
    internal ReadOnlySpan<char> Extract(
        ReadOnlySpan<char> tagContent, 
        out string[] values)
    {
        values = [ string.Empty ];

        if (tagContent.IndexOf('=') is 0)
        {
            tagContent = GetValues(tagContent[1..], out values);
        }

        return tagContent;
    }

    private static ReadOnlySpan<char> GetValues(
        ReadOnlySpan<char> valuesString, 
        out string[] values)
    {
        const string valueScreen = "\"'";

        var clipped = valuesString
            .Clip(valueScreen, valueScreen, true);

        var ranges = GetRanges(clipped)
            .Where(x => x.End.Value != 0);

        values = [..GetStrings(clipped, ranges)];
        return valuesString[clipped.Length..][2..];
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