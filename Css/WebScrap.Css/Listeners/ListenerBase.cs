using WebScrap.Tags;

namespace WebScrap.Css.Listeners;

internal abstract class ListenerBase
{
    internal abstract void Process(OpeningTag tag);
    internal abstract void Process(ClosingTag tag);
}