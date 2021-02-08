using Microsoft.CodeAnalysis;

namespace ComparableGenerator.UnitTest.Assertions
{
    public static class CompilationExtensions
    {
        public static CompilationAssertions Should(this Compilation compilation)
        {
            return new (compilation);
        }
    }
}