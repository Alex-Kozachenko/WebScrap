using WebScrap.Tags.Tools;

namespace WebScrap.Tags.Creators;

public abstract class TagCreatorBase
{
    public static TagBase Create(ReadOnlySpan<char> tag)
    {
        tag = tag.Clip("<", ">");
        return tag.IndexOf('/') switch
        {
            0 => new ClosingTagCreator().CreateTag(tag[1..]),
            var i 
                when i == (tag.Length - 1)
              => new InlineTagCreator().CreateTag(tag),
            _ => new OpeningTagCreator().CreateTag(tag)
        };
    }

    protected abstract TagBase CreateTag(ReadOnlySpan<char> tag);
}