WebScrap `v0.4`
=======

Features
=======

- Extract raw html by css selectors.
- Parse html table.

Please refer to [Features integration tests](/Api/WebScrap.API.Tests/Features/) for more details.

Usage
=======

HtmlExtraction
----------------
### Example

```csharp
var html = """
    <main>
        <br />
        <div>
            <p> LoremIpsum </p>
            <p id="foo"> 
                <div>
                    <span class="bar"> Two </span>
                    <span class="bar buzz"> Three </span>
                    <span id="four" class="bar buzz"> Four </span>
                </div>
            </p>
        </div>
    </main>
""";
var css = "main>div>p#foo span.bar";
var htmlEntries = Extract.Html(html, css);
// OUTPUT:
<span class="bar"> Two </span>
<span class="bar buzz"> Three </span>
<span id="four" class="bar buzz"> Four </span>
```

Known Issues
======

The project currently under active development, and there are some issues, some of the obvious, which are not the priority right now.

## CSS
- multiple css entries, comma-separated, are not supported.
- attribute-based css are not supported.

## HTML
- multiple root tags are not supported.
- object model returns tags in reverse order.

Please look for a [Known issues](https://github.com/search?q=repo%3AAlex-Kozachenko%2FWebScrap+KnownIssues.cs&type=code) tests sets, for actual list of current well-known issues.