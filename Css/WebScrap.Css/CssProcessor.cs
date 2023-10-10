using System.Collections.Immutable;
using WebScrap.Common.Tags.Creators;
using WebScrap.Common.Processors;
using WebScrap.Css.Listeners;

namespace WebScrap.Css;

/// <summary>
/// Processes the html with provided css-like-selectors,
/// and returns detected html which conforms 
/// the css from the parameter.
/// </summary>
public class CssProcessor
{
    private readonly List<int> tagIndexes = [];
    private readonly HtmlProcessor processor;

    public CssProcessor(
        TagFactoryBase tagFactory,
        ReadOnlySpan<char> css)
    {
        var tokens = Preprocessing.API.Process(css);
        var cssListener = new CssTagsListener(tokens);
        cssListener.CssComplianceMet += OnCompletedCssMet;
        processor = new HtmlProcessor(tagFactory, [cssListener]);
    }

    public void Run(ReadOnlySpan<char> html)
    {
        processor.Run(html);
    }

    public static ImmutableArray<int> CalculateTagIndexes(
        TagFactoryBase tagFactory, 
        ReadOnlySpan<char> html,
        ReadOnlySpan<char> css)
    {
        var processor = new CssProcessor(tagFactory, css);
        processor.Run(html);
        return [.. processor.tagIndexes];
    }

    private void OnCompletedCssMet(object? sender, EventArgs args)
    {
        tagIndexes.Add(processor.CharsProcessed);
    }
}