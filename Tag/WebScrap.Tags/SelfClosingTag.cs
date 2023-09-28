namespace WebScrap.Tags;

public sealed record class SelfClosingTag(
    string Name, 
    ILookup<string, string> Attributes) 
        : OpeningTag(Name, Attributes);