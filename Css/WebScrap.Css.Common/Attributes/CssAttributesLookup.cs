using System.Collections;

namespace WebScrap.Css.Common.Attributes;

public class CssAttributesLookup
    : ILookup<string, string>
{
    private readonly List<KeyValuePair<string, string>> attributes;

    public CssAttributesLookup() 
    {
        attributes = [];
    }

    public CssAttributesLookup(IList<KeyValuePair<string, string>> attributes)
    {
        this.attributes = [..attributes];
    }

    public void Add(KeyValuePair<char, string> selector)
    {
        var key = selector.Key switch
        {
            '#' => "id",
            '.' => "class",
            _ => throw new ArgumentException($"Unknown css: {selector.Key}{selector.Value}"),
        };
        attributes.Add(new(key, selector.Value));
    }

    public IEnumerable<string> this[string key]
        => attributes
            .Where(x => x.Key == key)
            .Select(x => x.Value)
            .ToArray();

    public int Count => attributes.Count;

    public bool Contains(string key)
        => attributes.Any(x => x.Key == key);

    public IEnumerator<IGrouping<string, string>> GetEnumerator()
        => attributes
            .ToLookup(x => x.Key, x => x.Value)
            .GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => attributes
            .ToLookup(x => x.Key, x => x.Value)
            .GetEnumerator();
}