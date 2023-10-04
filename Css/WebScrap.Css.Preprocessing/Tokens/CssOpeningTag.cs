using WebScrap.Common.Tags;

namespace WebScrap.Css.Preprocessing.Tokens;

public record class CssOpeningTag(
    string Name, // could be blank
    ILookup<string, string> Attributes, // could be blank
    char? ChildSelector) // null if root
    : OpeningTag(Name, Attributes);