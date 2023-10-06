using System.Collections.Immutable;
using WebScrap.Common.Tags;
using WebScrap.Common.Processors;
using WebScrap.Css.Common.Comparers;
using WebScrap.Css.Common.Tokens;
using WebScrap.Css.Traversing;

namespace WebScrap.Css.Listeners;

/// <summary>
/// </summary>
internal class CssTagsListener(ImmutableArray<CssTokenBase> expectedTags)
    : IProcessorListener
{
    public event EventHandler? Completed;
    public ImmutableArray<CssTokenBase> CssCompliantTags => expectedTags;

    public void Process(IReadOnlyCollection<OpeningTag> tagsHistory, OpeningTag tag)
    {
        var isCompleted = TraversingAPI.TraverseNames(expectedTags, tagsHistory) 
            && TraversingAPI.TraverseAttributes(expectedTags, tagsHistory);

        if (isCompleted)
        {
            Completed?.Invoke(this, EventArgs.Empty);
        }
    }

    public void Process(IReadOnlyCollection<OpeningTag> tagsHistory, ClosingTag tag)
    {
    }
}