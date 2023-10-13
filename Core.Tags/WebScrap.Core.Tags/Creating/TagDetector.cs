namespace WebScrap.Core.Tags.Creating;

internal class TagDetector(
    List<Action<UnprocessedTag>> unprocessedTagListeners,
    List<Action<ProcessedTag>> processedTagListeners,
    UnprocessedTagCreator unprocessedTagCreator,
    ProcessedTagCreator processedTagCreator)
{
    internal void Detect(ReadOnlySpan<char> tag)
    {
        if (!tag.StartsWith("<"))
        {
            throw new ArgumentException($"Html should start with tag. {tag}");
        }

        _ = TryProcessSelfClosingTag(tag) 
            || TryProcessClosingTag(tag) 
            || TryProcessOpeningTag(tag);
    }

    private bool TryProcessSelfClosingTag(ReadOnlySpan<char> tag)
    {
        var clippedTag = tag.Clip("<", ">", true);
        var isSelfClosingTag = clippedTag.LastIndexOf('/') == clippedTag.Length - 1;
        return isSelfClosingTag;
    }

    private bool TryProcessClosingTag(ReadOnlySpan<char> tag)
    {
        var clippedTag = tag.Clip("<", ">", true);
        if (clippedTag.IndexOf('/') != 0)
        {
            return false;
        }

        var result = processedTagCreator.Create(tag);
        processedTagListeners.ForEach(x => x(result));
        return true;
    }

    private bool TryProcessOpeningTag(ReadOnlySpan<char> tag)
    {
        var result = unprocessedTagCreator.Create(tag);
        unprocessedTagListeners.ForEach(x => x(result));
        return true;
    }
}