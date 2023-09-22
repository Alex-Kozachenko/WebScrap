HtmlStreamReader `v0.1`
=======
A bunch of html-processors at [Core.Processors](/Core/Processors/).

Features
=======

- `HtmlExtraction`: Extract html substrings for simple css-like queries.
    - supports only direct child selector, like `main>div`.

For full list of features, please refer to [Features integration tests](/Core.Tests.IntegrationTests/Features/).

Usage
=======

HtmlExtraction
----------------
Input: css, html.
```csharp
// Detect tags suitable for css parameter.
CssProcessor.CalculateTagIndexes(html, css)
    // Extract the ranges of detected tags.
    .Select(tagIndex => new Range(
        tagIndex,
        tagIndex + TagsProcessor.GetEntireTagLength(html.Substring(tagIndex))))
    // Return the actual strings from html.
    .Select(range => html[range])
    .ToArray()
```

### Example

```csharp
var css = "main>div>p";
var html = """
<main>
    <br />
    <div>
        <p>One</p>
    </div>
    <br />
    <div>
        <p>Two</p>
    </div>
</main>
""";

var htmlEntries = CssProcessor.CalculateTagIndexes(html, css)
    .Select(tagIndex => new Range(
        tagIndex,
        tagIndex + TagsProcessor.GetEntireTagLength(html.Substring(tagIndex))))
    .Select(range => html[range])
    .ToArray();

foreach (var html in htmlEntries)
{
    Console.WriteLine(html);
    // OUTPUT:
    // <p>One</p>
    // <p>Two</p>
}
```