using WebScrap.Css.Common.Selectors;
using WebScrap.Css.Preprocessing;

namespace WebScrap.Css.Preprocessing.Tests;

[TestFixture]
public class PreprocessingAPITests
{
    [Test]
    public void Process_WithDescendants_ShouldWork()
    {
        var sample = "main div>p";
        (Type, string)[] expected =
        [
            new(typeof(RootCssSelector), "main"),
            new(typeof(AnyChildCssSelector), "div"),
            new(typeof(ChildCssSelector), "p"),
        ];

        (Type, string)[] result = PreprocessingAPI.Process(sample)
            .Select(x => (x.Selector.GetType(), x.Tag.ToString()))
            .ToArray();
        Assert.That(result, Is.EquivalentTo(expected));
    }

    [Test]
    public void Process_WithMultipleSelectors_ShouldWork()
    {
        var sample = "p#foo.bar.buzz";
        var expected = new KeyValuePair<string, string>[]
        {
            new("id", "foo"),
            new("class", "bar"),
            new("class", "buzz"),
        }.ToLookup(x => x.Key, x => x.Value);

        var result = PreprocessingAPI.Process(sample)
            .First()
            .Attributes;

        Assert.That(result, Is.EquivalentTo(expected));
    }
}