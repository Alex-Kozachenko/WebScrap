namespace WebScrap.Common.Tags;

public record class OpeningTag(
    string Name,
    ILookup<string, string> Attributes)
        : TagBase(Name);