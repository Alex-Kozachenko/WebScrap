namespace  WebScrap.Css.Tests.Helpers;

public static class PointersHelper
{
    public static string[] ToSubstrings(this string pointers, string html)
    {
        var result = new List<int>();
        for (var i = 0; i < pointers.Length; i++)
        {
            var ch = pointers[i];
            if (ch == '^')
            {
                result.Add(i);
            }
        }
        return result.Select(x => html[x..]).ToArray();
    }
}