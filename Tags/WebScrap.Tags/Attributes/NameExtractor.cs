namespace WebScrap.Tags.Attributes;

public sealed class NameExtractor : IExtractor<string>
{
    public ReadOnlySpan<char> Extract(
        ReadOnlySpan<char> tagContent, 
        out string key) 
        {
            const string nameDelimeters = " =";
            key = tagContent.IndexOfAny(nameDelimeters) switch
            {
                -1 => tagContent.ToString(),
                var index => tagContent[..index].ToString()
            };
            return tagContent[key.Length..];
        } 
}