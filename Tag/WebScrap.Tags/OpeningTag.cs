namespace WebScrap.Tags;

//TODO: please read https://learn.microsoft.com/en-us/dotnet/api/system.linq.lookup-2?view=net-7.0&redirectedfrom=MSDN

public sealed record class OpeningTag(
    ReadOnlyMemory<char> Name,
    ILookup<string, string> Attributes,
    ReadOnlyMemory<char> InnerText)
        : TagBase(Name)
{
    public static new OpeningTag Create(ReadOnlySpan<char> content)
    {
        var name = content[..content.IndexOf(' ')];
        content = content[content.IndexOf(' ')..][1..];
        return CreateWithAttributes(content, name);
    }

    private static OpeningTag CreateWithAttributes(
        ReadOnlySpan<char> content,
        ReadOnlySpan<char> name)
    {
        var attributes = new List<string>();
        var (key, values) = content.IndexOf('=') switch
        {
            -1 => (content.ToString(), [""]),
            var i => (content[..i].ToString(), ExtractAttributeValues(content[i..])),
        };
        foreach (var value in values)
        {
            attributes.Add(value);
        }


        return new OpeningTag(
            Name: name.ToString().AsMemory(),
            Attributes: attributes.ToLookup(x => key, x => x),
            InnerText: "".AsMemory());
    }

    // REALLY needs RANGES
    private static string[] ExtractAttributeValues(
        ReadOnlySpan<char> content)
    {
        const string valueScreen = "\"'";
        content = Clip(content, valueScreen, valueScreen);
        const int maxAttributeValuesCount = 100;
        Span<Range> ranges = stackalloc Range[maxAttributeValuesCount];
        if (content.Split(ranges, ' ') is 0)
        {
            return [content.ToString()];
        }

        var result = new List<String>();
        foreach (var range in ranges)
        {
            if (range.End.Value != 0)
            {
                result.Add(content[range].ToString());
            }
        }

        return [.. result];
    }
}
