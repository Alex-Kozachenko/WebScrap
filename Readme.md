WebScrap `v0.3`
=======

Features
=======

## Extract html with simple css-like queries
- Id and Class selectors.
- Deep descendance.


For full list of features, please refer to [Features integration tests](/WebScrap.Tests.IntegrationTests/Features/).

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

Please refer to a [API test set](./Api/WebScrap.API.Tests/) for more usecases.

Known Issues
======
Please look for a [Known issues](https://github.com/search?q=repo%3AAlex-Kozachenko%2FWebScrap+KnownIssues_Tests.cs&type=code) tests sets, for actual list of current well-known issues.
