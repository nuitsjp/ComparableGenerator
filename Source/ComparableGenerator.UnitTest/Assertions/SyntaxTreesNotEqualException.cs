using Microsoft.CodeAnalysis;
using Xunit.Sdk;

namespace ComparableGenerator.UnitTest.Assertions
{
    public class SyntaxTreesNotEqualException : AssertActualExpectedException
    {
        public SyntaxTreesNotEqualException(
            SyntaxTree expected,
            SyntaxTree actual)
            : base(expected, actual, "Generated SyntaxTree differs from the expected one.", "Expected SyntaxTree", "Actual SyntaxTree") { }
    }
}