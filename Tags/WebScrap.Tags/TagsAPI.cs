using WebScrap.Common.Contracts;
using WebScrap.Common.Tags;

namespace WebScrap.Tags;

public static class TagsAPI
{
    public static TagFactoryBase CreateTagFactory()
        => new TagFactory();
}