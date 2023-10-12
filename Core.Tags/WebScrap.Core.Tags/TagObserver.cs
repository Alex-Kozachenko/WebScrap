namespace WebScrap.Core.Tags.Creators;

internal class TagObserver(
    List<Action<OpenedTag>> openedTagListeners, 
    List<Action<ProcessedTag>> processedTagListeners)
{
    public void Run(ReadOnlySpan<char> tag, OpenedTag? lastOpenedTag, int charsProcessed)
    {
        if (!tag.StartsWith("<"))
        {
            throw new ArgumentException($"Html should start with tag. {tag}");
        }

        var clippedTag = tag.Clip("<", ">");
        var isSelfClosingTag = clippedTag.LastIndexOf('/') == clippedTag.Length - 1;
        if (isSelfClosingTag)
        {
            return;
        }

        var isClosingTag = clippedTag.IndexOf('/') == 0;
        if (isClosingTag && lastOpenedTag != null)
        {
            clippedTag = clippedTag[1..];
            var processedTag = new ProcessedTagCreator()
                .CreateProcessedTag(clippedTag, lastOpenedTag, charsProcessed);

            processedTagListeners.ForEach(x => x(processedTag));
        }
        else
        {
            var openedTag = new OpenedTagCreator()
                .CreateOpenedTag(clippedTag, charsProcessed);

            openedTagListeners.ForEach(x => x(openedTag));
        }
    }
}