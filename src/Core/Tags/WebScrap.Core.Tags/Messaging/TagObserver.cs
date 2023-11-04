using WebScrap.Core.Tags.Data;

namespace WebScrap.Core.Tags.Messaging;

internal record class TagObserver(IObserver<TagsProviderMessage> Observer, string? TagName);
