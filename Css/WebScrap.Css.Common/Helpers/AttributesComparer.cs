namespace WebScrap.Css.Common.Helpers;

public class AttributesComparer(
    ILookup<string, string> SubSet,
    ILookup<string, string> SuperSet)
{
    /// <summary>
    /// Checks if <see cref="SuperSet"/> lookup contains all keys and values, 
    /// which are required by <see cref="SubSet"/>.
    /// </summary>
    public static bool IsSubsetOf(
        ILookup<string, string> subset,
        ILookup<string, string> superset)
        => new AttributesComparer(subset, superset)
            .IsSubset();

    private bool IsSubset()
    {
        foreach (var subAttr in SubSet)
        {
            if (!SuperSet.Contains(subAttr.Key))
            {
                return false;
            }

            var superValues = SuperSet[subAttr.Key].ToArray();
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