namespace WebScrap.Core.Tags.Data;

/// <summary>
/// Represents the html info about the tag.
/// </summary>
public record class TagInfo(string Name, ILookup<string, string> Attributes);