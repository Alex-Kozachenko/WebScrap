namespace WebScrap.Core.Tags.Creating;

internal class TagCreatorManager(
    UnprocessedTagCreator unprocessedTagCreator,
    ProcessedTagCreator processedTagCreator)
{
    internal void Run(ReadOnlySpan<char> tag)
    {
        if (!tag.StartsWith("<"))
        {
            throw new ArgumentException($"Html should start with tag. {tag}");
        }

        var clippedTag = tag.Clip("<", ">", true);
        var isSelfClosingTag = clippedTag.LastIndexOf('/') == clippedTag.Length - 1;
        if (isSelfClosingTag)
        {
            return;
        }

        var isClosingTag = clippedTag.IndexOf('/') == 0;
        if (isClosingTag)
        {
            processedTagCreator.Run(tag);
        }
        else
        {
            unprocessedTagCreator.Run(tag);
        }
    }
}