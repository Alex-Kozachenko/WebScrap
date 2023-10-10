// using System.Collections.Immutable;
// using WebScrap.Common.Css;
// using WebScrap.Html.Extracting.Listeners;

// namespace WebScrap.Html.Extracting;

// public static class ExtractingAPI
// {
//     public static class Extract
//     {
//         public static ImmutableArray<int> TagIndexes(
//             TagFactoryBase tagFactory, 
//             IProcessorListener[] listeners,
//             ReadOnlySpan<char> html)
//         {
//             HtmlProcessor processor = null;
//             var tagIndexes = new List<int>();
//             var cssListener = new CssMatchListener(expectedTags);
//             cssListener.CssComplianceMet += 
//                 (o, e) => tagIndexes.Add(processor.CharsProcessed);

//             // HACK: mutually coupled.
//             processor = new HtmlProcessor(tagFactory, [cssListener]);
//             processor.Run(html);
//             return [.. tagIndexes];
//         }
//     }
// }
