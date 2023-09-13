using System.Text;
using static Core.Internal.HtmlProcessing.TagNavigator;

namespace Core.Internal.HtmlProcessing;

internal static class TextExtractor
{
    internal static string Extract(ReadOnlySpan<char> html)
    {
        var result = new StringBuilder();
        while (html.Length is not 0)
        {
            var body = html
                .GoToTagBody();

            if (body.Length is 0)
            {
                break;
            }
            result.Append(body.ReadInnerText());
            html = body.GoToNextTag();
        }
        return result.ToString();
    }

    private static ReadOnlySpan<char> ReadInnerText(this ReadOnlySpan<char> tagBody)
        => tagBody.IndexOf('<') switch
        {
            -1 => tagBody[..^1],
            var nextTagIndex => tagBody[..nextTagIndex],
        };
}