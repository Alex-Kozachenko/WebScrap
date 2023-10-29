using DevOvercome.WebScrap;

var request = "https://www.gpucheck.com/gpu-benchmark-graphics-card-comparison-chart";

// Download the html:
using var client = new HttpClient();
using var response = await client.GetAsync(request);
var html = await response.Content.ReadAsStringAsync();

// Run the WebScrapper:
var css = "table";
var resultJson = new WebScrapper(html)
    .Run(css)
    .AsJson();

var result = resultJson.ToJsonString();

// Get the results:
Console.WriteLine(result);
File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "result.json", result);
// OUTPUT:
// [{"key":"#firstHeading","values":[{"value":"Food energy"}]}]
Console.Read();