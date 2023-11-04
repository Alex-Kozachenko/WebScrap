using WebScrap.Core.Tags.Data;

namespace WebScrap.Core.Tags.Messaging;

public interface ITagObservable : IObservable<TagsProviderMessage>
{
    public IDisposable Subscribe(IObserver<TagsProviderMessage> observer, params string[] tagNames);
}
