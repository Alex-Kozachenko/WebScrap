using System.Text;
using static Core.Tools.TagNavigator;

namespace Core.Tools;

internal static class TextExtractor
{
    internal static string Extract(ReadOnlySpan<char> html)
    {
        var result = new StringBuilder();
        while (html.Length is not 0)
        {
            var nextTag = html.GoToNextTag();
            if (nextTag.Length is 0)
            {
                break;
            }
            var innerText = nextTag.GrabInnerText();
            result.Append(innerText);
            html = nextTag;
        }
        return result.ToString();
    }
}