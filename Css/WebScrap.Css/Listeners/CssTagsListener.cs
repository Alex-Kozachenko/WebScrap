using WebScrap.Common.Tags;
using WebScrap.Common.Processors;

using WebScrap.Css.Traversing;
using WebScrap.Css.Common;

namespace WebScrap.Css.Listeners;

internal class CssTagsListener(CssToken[] expectedTags)
    : IProcessorListener
{
    public event EventHandler? CssComplianceMet;

    public void Process(IReadOnlyCollection<OpeningTag> tagsHistory, OpeningTag tag)
    {
        var isEntireCssMet = TraversingAPI.TraverseNames(expectedTags, tagsHistory) 
            && TraversingAPI.TraverseAttributes(expectedTags, tagsHistory);

        if (isEntireCssMet)
        {
            CssComplianceMet?.Invoke(this, EventArgs.Empty);
        }
    }

    public void Process(IReadOnlyCollection<OpeningTag> tagsHistory, ClosingTag tag)
    {
    }
}