using NUnit.Framework.Internal;
using static Core.Tools.TextExtractor;

namespace Core.Tests;

[TestFixture]
public class TextExtractorTests
{
    [Test]
    public void Extract_ShouldRead_InnerText_In_NestedTags()
    {
        var html = """
            <p> One
                <p> Two
                    One Two <span> Three </span> <b> Four </b>
                </p>
            </p>
        """;

        var expected = "One Two One Two Three Four";
        var result = Extract(html);
        result = result.Strip();
        Assert.That(result, Is.EqualTo(expected));
    }

    
}