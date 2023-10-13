# WebScrap

A `HTML` parser, for extracting the text from a web pages, with `CSS` selectors.

## Usage

1. Use the `WebScrap.API` namespace as entry point.
1. Call the `Extract.Html` with `html` and `css`.
1. Get the raw html as result.

```csharp
using WebScrap.API;

var css = ".bar";
var html = """
    <main>
        <div>
            <span class="bar"> Lorem </span>
        </div>
        <p class="bar buzz"> Lorem </span>
    </main>
""";

var htmlEntries = Extract.Html(html, css);
// OUTPUT:
<span class="bar"> Lorem </span>
<p class="bar buzz"> Lorem </span>
```

## Known Issues

The project currently under active development, and there are some issues, some of the obvious, which are not the priority right now.

### CSS
- multiple css entries, comma-separated, are not supported.
- attribute-based css are not supported.

### HTML
- multiple root tags are not supported.
- object model returns tags in reverse order.

Please look for a [Known issues](https://github.com/search?q=repo%3AAlex-Kozachenko%2FWebScrap+KnownIssues.cs&type=code) tests sets, for actual list of current well-known issues.

## Solution structure

The solution consists of lesser projects, which are domain-based, core and API.

### Domains

- [Html](./Html/) is all about html parsing.
- [Css](./Css/) is all about processing the css query and getting the html tags.

### Core

- [Core.Tags](./Core.Tags/) contains the html processor, which is designed to be used in Domains.

### API

- [API](./Api) integrated the `Domains` into single and simple facade, for use.

## Goals

This project is for extracting text from html in a performant way.

### Extract text

* `Plain text`: This tool must extract a plain text from html.
* `User-defined result structure`: The amount of text, and it's structure is defined by user, via multiple css selectors.

### Performance

- `Parallel processing`: All of css selectors should process the html in parallel.
- `Stream-based processing`: The processed parts of html should be disposed from memory.

## Footnote

Please refer to the [Changelog](./Changelog.md) for the progress.