namespace WebScrap.Common.Tags;

public sealed record class InlineTag(
    string Name, 
    ILookup<string, string> Attributes) 
        : OpeningTag(Name, Attributes);