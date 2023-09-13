using NUnit.Framework.Internal;
using Core.Internal.HtmlProcessing;

namespace Core.Internal.HtmlProcessing.Tests;

[TestFixture]
public class TagNavigatorTests
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
        "     ^"
    )]
    [TestCase(
        "__</p>__",
        "  ^"
    )]
    #endregion
    public void GoToNextTag_ShouldWork_LikeThat(string sample, string targetPointer)
    {
        var expectedResult = sample[(targetPointer.Length - 1)..];
        var result = TagNavigator.GoToNextTag(sample)
            .ToString();
        Assert.That(result, Is.EquivalentTo(expectedResult));
    }

    #region cases
    [TestCase(
        "</p>__",
        "    ^"
    )]
    [TestCase(
        "<p>__",
        "   ^"
    )]
    [TestCase(
        "___<p>",
        "^"
    )]
    #endregion
    public void GoToTagBody_ShouldWork_LikeThat(string sample, string targetPointer)
    {
        var expectedResult = sample[(targetPointer.Length - 1)..];
        var result = TagNavigator.GoToTagBody(sample)
            .ToString();
        Assert.That(result, Is.EquivalentTo(expectedResult));
    }
}