using NUnit.Framework.Constraints;

namespace Core.IntegrationTests.TestHelpers;

internal static class IsHelpers
{
    internal static CollectionEquivalentConstraint EquivalentTo(string[] expected)
        => Is.EquivalentTo(expected);
}