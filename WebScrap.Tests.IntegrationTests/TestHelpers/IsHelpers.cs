using NUnit.Framework.Constraints;

namespace WebScrap.IntegrationTests.TestHelpers;

internal static class IsHelpers
{
    internal static CollectionEquivalentConstraint EquivalentTo(string[] expected)
        => Is.EquivalentTo(expected);
}