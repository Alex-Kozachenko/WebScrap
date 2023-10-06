using WebScrap.Common.Tags;

namespace WebScrap.Common.Processors;

public interface IProcessorListener
{
    public void Process(IReadOnlyCollection<OpeningTag> history, OpeningTag tag);
    public void Process(IReadOnlyCollection<OpeningTag> history, ClosingTag tag);
}