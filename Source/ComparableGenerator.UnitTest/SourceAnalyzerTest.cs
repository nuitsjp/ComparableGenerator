using System.Threading.Tasks;
using ComparableGenerator.UnitTest.Assertions;
using VerifyCS = ComparableGenerator.UnitTest.Verifiers.CSharpAnalyzerVerifier<
    ComparableGenerator.SourceAnalyzer>;

namespace ComparableGenerator.UnitTest
{
    public class SourceAnalyzerTest : UnitTestBase
    {
        public override async Task Should_not_be_generated_for_CompareAttribute_is_not_defined(string source)
        {
            await source.CreateAnalyzer()
                .Should().BeEmpty()
                .VerifyAnalyzerAsync();
        }

        public override async Task Should_be_generated_for_class(string source)
        {
            await source.CreateAnalyzer()
                .Should().BeEmpty()
                .VerifyAnalyzerAsync();
        }

        public override async Task Should_be_generated_for_struct(string source)
        {
            await source.CreateAnalyzer()
                .Should().BeEmpty()
                .VerifyAnalyzerAsync();
        }

        public override async Task Should_not_be_generated_When_not_exists_CompareBy(string source)
        {
            await source.CreateAnalyzer()
                .Should().Contain(SourceAnalyzer.CompareByIsNotDefined.Rule)
                    .WithLocation(7, 18)
                    .WithArguments("MyNamespace", "MyClass")
                .VerifyAnalyzerAsync();
        }

        public override async Task Should_not_be_generated_When_not_exists_Compare(string source)
        {
            #region FixedCode
            var fixedCode = @"
using ComparableGenerator;

namespace MyNamespace
{
[Comparable]    public class MyClass
    {
        [CompareBy]
        public int Value { get; set; }
    }
}
";
            #endregion
            await source.CreateAnalyzer()
                .Should().Contain(SourceAnalyzer.CompareIsNotDefined.Rule)
                    .WithLocation(6, 18)
                    .WithArguments("MyNamespace", "MyClass")
                    .WithCodeFix(fixedCode)
                .VerifyCodeFixAsync();
        }
    }
}
