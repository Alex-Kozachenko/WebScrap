WebScrap `v0.2`
=======
A bunch of html-processors at [WebScrap.Processors](/WebScrap/Processors/).

Features
=======

- `HtmlExtraction`: Extract html substrings for simple css-like queries.


For full list of features, please refer to [Features integration tests](/WebScrap.Tests.IntegrationTests/Features/).

Usage
=======

HtmlExtraction
----------------
### Example

```csharp
    var css = "main>div>p.foo";
    var html = """
    <main>
        <br />
        <div>
            <p class="foo bar">One</p>
        </div>
        <br />
        <div>
            <p class="foo buzz">Two</p>
        </div>
    </main>
    """;

    var htmlEntries = API.ExtractHtml(html, css);

    string[] expected = [
        """<p class="foo bar">One</p>""",
        """<p class="foo buzz">Two</p>"""
    ];
    Assert.That(htmlEntries, Is.EquivalentTo(expected));
```

Please refer to a [API test set](./Api/WebScrap.API.Tests/) for more usecases.

Known Issues
======
Please refer to a [KnownIssues test set](./Api/WebScrap.API.KnownIssues.Tests/) for actual list of current well-known issues.