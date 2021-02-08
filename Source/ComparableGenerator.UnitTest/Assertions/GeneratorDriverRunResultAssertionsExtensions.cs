using Microsoft.CodeAnalysis;

namespace ComparableGenerator.UnitTest.Assertions
{
    public static class GeneratorDriverRunResultAssertionsExtensions
    {
        public static GeneratorDriverRunResultAssertions Should(this GeneratorDriverRunResult generatorDriverRunResult)
            => new (generatorDriverRunResult);
    }
}