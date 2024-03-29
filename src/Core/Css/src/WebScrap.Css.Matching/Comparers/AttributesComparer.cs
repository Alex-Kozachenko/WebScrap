using WebScrap.Core.Tags;
using WebScrap.Core.Tags.Data;
using WebScrap.Css.Data;

namespace WebScrap.Css.Matching.Comparers;

public class AttributesComparer : IComparer
{
    public bool AreSame(CssToken css, TagInfo tagInfo) 
        => IsSubset(
            css.Attributes, 
            tagInfo.Attributes);

    /// <summary>
    /// Checks if <see cref="SuperSet"/> lookup contains all keys and values, 
    /// which are required by <see cref="SubSet"/>.
    /// </summary>
    private static bool IsSubset(
        ILookup<string, string> subset,
        ILookup<string, string> superset)
    {
        foreach (var subAttr in subset)
        {
            if (!superset.Contains(subAttr.Key))
            {
                return false;
            }

            var superValues = superset[subAttr.Key].ToArray();
            foreach (var value in subAttr)
            {
                if (!superValues.Contains(value))
                {
                    return false;
                }
            }
        }
        return true;
    }
}