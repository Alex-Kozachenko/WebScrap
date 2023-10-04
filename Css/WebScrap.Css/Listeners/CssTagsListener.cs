using WebScrap.Common.Tags;
using WebScrap.Css.Listeners.Helpers;
using WebScrap.Css.Preprocessing.Tokens;

namespace WebScrap.Css.Listeners;

/// <summary>
/// Tracks specific CSS-like query.
/// </summary>
internal class CssTagsListener(ReadOnlySpan<char> css) : ListenerBase
{
    private readonly CssTracker cssTracker = new(css);
    private readonly Stack<CssTokenBase> cssTags = new();
    public event EventHandler? Completed;

    public Stack<CssTokenBase> CssCompliantTags => new (cssTags.Reverse());

    internal override void Process(OpeningTag tag)
    {
        if (IsNameMet(tag) && IsAttrMet(tag))
        {
            var css = cssTracker.GetCurrentExpectedTag(cssTags.Count);
            cssTags.Push(css);
        }

        if (cssTracker.IsCompletedCssMet(cssTags.Count))
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
        var css = cssTracker.GetCurrentExpectedTag(cssTags.Count);
        return tag.Name.Equals(css.Name);
    }

    private bool IsAttrMet(OpeningTag tag)
    {
        var css = cssTracker.GetCurrentExpectedTag(cssTags.Count);
        var isAttrRequired = css.Attributes.Any(x => x.Key != "");
        return !isAttrRequired
            || AttributesComparer.IsSubsetOf(css.Attributes, tag.Attributes);
    }
}