namespace WebScrap.Css.Listeners.Helpers;

internal class AttributesComparer(
    ILookup<string, string> source,
    ILookup<string, string> dest)
{

    /// <summary>
    /// Checks if <see cref="dest"/> lookup contains all keys and values, 
    /// which are required by <see cref="source"/>.
    /// </summary>
    internal static bool IsSubset(
        ILookup<string, string> source,
        ILookup<string, string> dest)
        => new AttributesComparer(source, dest)
            .IsSubset();

    private bool IsSubset()
        => IsDestBigger() 
        && AreKeysSubset() 
        && AreValuesSubset();

    private bool IsDestBigger()
        => dest.Count >= source.Count;

    private bool AreKeysSubset()
    {
        foreach (var attr in dest)
        {
            if (!source.Contains(attr.Key))
            {
                return false;
            }
        }
        return true;
    }

    private bool AreValuesSubset()
    {
        foreach (var attr in dest)
        {
            foreach (var value in attr)
            {
                if (!source[attr.Key].ToArray().Contains(value))
                {
                    return false;
                }
            }
        }

        return true;
    }
}