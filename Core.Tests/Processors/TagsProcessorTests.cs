using NUnit.Framework.Internal;
using static Core.Processors.TagsProcessor;

namespace Core.Processors.Tests;

[TestFixture]
public class TagsProcessorTests
{
    [Test]
    public void ExtractEntireTag_SingleTag_ShouldWork()
    {
        var expected = """
            <p>
                Lorem ipsum
            </p>
        """;

        var html = expected + """
            Lorem ipsum...
            <div> <p> Lorem ipsum </p> </div>            
        """;

        var actual = ExtractEntireTag(html).ToString();
        Assert.That(actual, Is.EquivalentTo(expected));
    }

    [Test]
    public void ExtractEntireTag_NestedTag_ShouldWork()
    {
        var expected = """
            <div>
                <p>
                    <b> Lorem ipsum </b>
                </p>
            </div>
        """;

        var html = expected + """
            Lorem ipsum...
            <div> <p> Lorem ipsum </p> </div>            
        """;

        var actual = ExtractEntireTag(html).ToString();
        Assert.That(actual, Is.EquivalentTo(expected));
    }
}