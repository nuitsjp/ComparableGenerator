using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Xunit;
using VerifyCS = ComparableGenerator.UnitTest.Verifiers.CSharpAnalyzerVerifier<
    ComparableGenerator.SourceAnalyzer>;

namespace ComparableGenerator.UnitTest
{
    public class SourceAnalyzerTest : UnitTestBase
    {
        public override async Task Should_be_generated_for_class(string inputCompilation)
        {
            await VerifyCS.VerifyAnalyzerAsync(inputCompilation);
        }

        public override async Task Should_be_generated_for_struct(string inputCompilation)
        {
            await VerifyCS.VerifyAnalyzerAsync(inputCompilation);
        }

        [Theory]
        [InlineData(@"
using ComparableGenerator;

namespace MyNamespace
{
    [Comparable]
    public class MyClass
    {
    }
}
")]
        public override async Task Should_not_be_generated_When_not_exists_CompareBy(string source)
        {
            var expected = VerifyCS.Diagnostic(SourceAnalyzer.Rule).WithLocation(6, 5);
            await VerifyCS.VerifyAnalyzerAsync(source, expected);
        }
    }
}