namespace WebScrap.Modules.Export.Json.Tests;

public class JsonApiTests
{
    [Test]
    public void Test1()
    {
        var html = "<div>foo</div>".AsMemory();
        var expected = """[{"value":"foo"}]""";
        var actual = JsonApi.Export([html]).ToJsonString();
        Assert.That(expected, Is.EquivalentTo(actual));
    }
}