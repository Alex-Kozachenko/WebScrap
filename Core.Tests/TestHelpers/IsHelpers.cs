using NUnit.Framework.Constraints;

namespace Core.Tests.TestHelpers;

internal static class IsHelpers
{
    internal static CollectionEquivalentConstraint EquivalentTo(string[] expected)
        => Is.EquivalentTo(expected);
}