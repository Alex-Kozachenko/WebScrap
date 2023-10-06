using System.Collections.Immutable;
using WebScrap.Common.Tags;
using WebScrap.Css.Common.Comparers;
using WebScrap.Css.Common.Tokens;

namespace WebScrap.Css.Listeners;

/// <summary>
/// Tracks specific CSS-like query.
/// </summary>
internal class CssTagsListener(ImmutableArray<CssTokenBase> expectedTags) 
    : ListenerBase
{
    private readonly Stack<CssTokenBase> cssTags = new();
    public event EventHandler? Completed;

    public Stack<CssTokenBase> CssCompliantTags => new (cssTags.Reverse());

    internal override void Process(OpeningTag tag)
    {
        if (IsNameMet(tag) && IsAttrMet(tag))
        {
            var css = GetCurrentExpectedTag(cssTags.Count);
            cssTags.Push(css);
        }

        if (IsCompletedCssMet(cssTags.Count))
        {
            Completed?.Invoke(this, EventArgs.Empty);
        }
    }

    internal override void Process(ClosingTag tag)
    {
        if (IsNameMet(tag))
        {
            //HACK: it could mask an error.
            cssTags.TryPop(out var result);
        }
    }

    private bool IsNameMet(TagBase tag)
    {
        var css = GetCurrentExpectedTag(cssTags.Count);
        return new NameComparer().AreSame(css, tag.Name);
    }

    private bool IsAttrMet(OpeningTag tag)
    {
        var css = GetCurrentExpectedTag(cssTags.Count);
        var isAttrRequired = css.Attributes.Any(x => x.Key != "");
        return !isAttrRequired
             || new AttributesComparer().AreSame(css, tag);
    }

    private CssTokenBase GetCurrentExpectedTag(int processedTagsCount)
    {
        var index = processedTagsCount switch
        {
            < 0 => throw new ArgumentOutOfRangeException(
                $"{nameof(processedTagsCount)} = {processedTagsCount}"),
            var i when i < expectedTags.Length
               => i,
            _ => expectedTags.Length - 1,
        };

        return expectedTags[index];
    }

    private bool IsCompletedCssMet(int cssTagsLength)
        => cssTagsLength == expectedTags.Length;
}