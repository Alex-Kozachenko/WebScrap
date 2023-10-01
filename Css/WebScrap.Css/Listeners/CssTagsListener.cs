using WebScrap.Tags;
using WebScrap.Css.Listeners.Helpers;

namespace WebScrap.Css.Listeners;

/// <summary>
/// Tracs specific CSS-like query.
/// </summary>
internal class CssTagsListener(ReadOnlySpan<char> css) : ListenerBase
{
    private readonly CssTracker cssTracker = new(css);
    private readonly Stack<OpeningTag> cssTags = new();
    public event EventHandler? Completed;

    public Stack<OpeningTag> CssCompliantTags => new (cssTags.Reverse());

    internal override void Process(OpeningTag tag)
    {
        if (IsNameMet(tag) && IsAttrMet(tag))
        {
            cssTags.Push(tag);
        }

        if (cssTracker.IsCompletedCssMet(cssTags))
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
        var css = cssTracker.GetCurrentExpectedTag(cssTags);
        return tag.Name.AsSpan()
            .SequenceEqual(css.Tag.Span);
    }

    private bool IsAttrMet(OpeningTag tag)
    {
        var css = cssTracker.GetCurrentExpectedTag(cssTags);
        var isAttrRequired = css.Attributes.Any(x => x.Key != "");
        return !isAttrRequired
            || AttributesComparer.IsSubset(tag.Attributes, css.Attributes);
    }
}