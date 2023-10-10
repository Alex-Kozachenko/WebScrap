using WebScrap.Css.Common.Selectors;
using WebScrap.Css.Preprocessing;

namespace WebScrap.Css.Preprocessing.Tests;

[TestFixture]
public class PreprocessingAPITests
{
    [Test]
    public void Read_ShouldProcess_Descendants()
    {
        var sample = "main div>p";
        (Type, string)[] expected =
        [
            new(typeof(RootCssSelector), "main"),
            new(typeof(AnyChildCssSelector), "div"),
            new(typeof(ChildCssSelector), "p"),
        ];

        (Type, string)[] result = API.Process(sample)
            .Select(x => (x.Selector.GetType(), x.Tag.ToString()))
            .ToArray();
        Assert.That(result, Is.EquivalentTo(expected));
    }

    [Test]
    public void Read_ShouldProcess_MultipleSelectors()
    {
        var sample = "p#foo.bar.buzz";
        var expected = new KeyValuePair<string, string>[]
        {
            new("id", "foo"),
            new("class", "bar"),
            new("class", "buzz"),
        }.ToLookup(x => x.Key, x => x.Value);

        var result = API.Process(sample)
            .First()
            .Attributes;

        Assert.That(result, Is.EquivalentTo(expected));
    }
}