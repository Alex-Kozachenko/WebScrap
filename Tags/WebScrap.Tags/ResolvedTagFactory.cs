using WebScrap.Common.Tags;
using WebScrap.Common.Tags.Creators;
using WebScrap.Tags.Creators;

namespace WebScrap.Tags;

public class ResolvedTagFactory() : TagFactory(new TagCreatorResolver())
{
}