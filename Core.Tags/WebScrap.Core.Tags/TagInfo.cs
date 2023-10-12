namespace WebScrap.Core.Tags;

public record class TagInfo(string Name, ILookup<string, string> Attributes);