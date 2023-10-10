namespace WebScrap.API;

public static class Parse
{
    public static string[][] Table(
        ReadOnlySpan<char> html, 
        ReadOnlySpan<char> css)
    {
        var table = Extract.Html(html, css);
        var headers = Extract.Html(table[0], "tr>th").ToArray();
        var values = Extract.Html(table[0], "tr>td").ToArray();

        var result = new List<string[]>
        {
            headers
        };

        var buffer = new string[headers.Length];
        for (int i = 0; i < values.Length; i++)
        {
            var j = i % headers.Length;
            buffer[j] = values[i];
            if (j == headers.Length - 1)
            {
                result.Add([..buffer]);
            }
        }

        return [.. result];
    }
}