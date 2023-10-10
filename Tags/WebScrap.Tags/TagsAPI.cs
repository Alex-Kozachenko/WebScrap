using WebScrap.Common.Contracts;

namespace WebScrap.Tags;

public static class TagsAPI
{
    public static TagFactoryBase CreateTagFactory()
        => new TagFactory();
}