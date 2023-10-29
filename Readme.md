# WebScrap

A `HTML` parser, for extracting the text from a web pages, with `CSS` selectors.

## Purpose

The purpose of this library is to get the **essential data** from a web-page for a user, in `JSON` format.

It could be further used for:
1. Analyzing the **essential data**. Like a charts, diagramms, plain tables.
2. Tracking the history of the **essential data**. Like prices for sales, currencies, user activity.
3. Searching for specific **essential data**. Some word in multiple html resources, like movie title, or any other product, any mentioning.

## Usage

Let's make a console demo:
> ` dotnet new console -n WebScrap.Demo.CLI `
> ` cd WebScrap.Demo.CLI `

Install the package:
> ` dotnet add package DevOvercome.WebScrap --version 1.0.0-rc.5 `

And try the following code:
```csharp
using DevOvercome.WebScrap;

var request = "https://en.wikipedia.org/wiki/Food_energy";

// Download the html:
using var client = new HttpClient();
using var response = await client.GetAsync(request);
var html = await response.Content.ReadAsStringAsync();

// Run the WebScrapper:
var css = "#firstHeading";
var result = new WebScrapper(html)
    .Run(css)
    .AsJson()
    .ToJsonString();

// Get the results:
Console.WriteLine(result);
// OUTPUT:
// [{"key":"#firstHeading","values":[{"value":"Food energy"}]}]
Console.Read();
```

## Known Issues

The project currently under active development, and there are some issues, some of the obvious, which are not the priority right now.

### Global
- `No SDK found`: VSCode reports "No SDK found" if second workspace is opened. @2023.11.19

### CSS
- multiple css entries, comma-separated, are not supported.
- attribute-based css are not supported.

### HTML
- multiple root tags are not supported.
- object model returns tags in reverse order.

Please look for a [Known issues](https://github.com/search?q=repo%3AAlex-Kozachenko%2FWebScrap+KnownIssues.cs&type=code) tests sets, for actual list of current well-known issues.

## Goals

This project is for extracting text from html in a performant way.

### Extract text

* `Plain text`: This tool must extract a plain text from html.
* `User-defined result structure`: The amount of text, and it's structure is defined by user, via multiple css selectors.

### Performance

- `Parallel processing`: All of css selectors should process the html in parallel.
- `Stream-based processing`: The processed parts of html should be disposed from memory.

## Footnote

- The versioning is complied to the Semver 2.0.0. Please refer to [semver.org](https://semver.org/) for details.
- Please refer to the [Changelog](./Changelog.md) for the progress.
