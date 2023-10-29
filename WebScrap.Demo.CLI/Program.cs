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