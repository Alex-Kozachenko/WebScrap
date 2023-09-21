using static Core.Tools.Html.TagsNavigator;

namespace Core.Tools.Html.Tests;

[TestFixture]
public class TagsNavigatorTests
{
    #region cases
    [TestCase(
        "_____<p></p>",
        "     ^"
    )]
    [TestCase(
        "p>__<p>",
        "    ^"
    )]
    [TestCase(
        "<p>__<p>",
        "^"
    )]
    [TestCase(
        "__</p>__",
        "  ^"
    )]
    #endregion
    public void GetNextTagIndex_ShouldWork(string sample, string targetPointer)
    {
        var expected = targetPointer.Length - 1;
        var result = GetNextTagIndex(sample);
        Assert.That(result, Is.EqualTo(expected));
    }
}