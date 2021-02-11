using System.Threading.Tasks;
using ComparableGenerator.UnitTest.Assertions;
using VerifyCS = ComparableGenerator.UnitTest.Verifiers.CSharpAnalyzerVerifier<
    ComparableGenerator.SourceAnalyzer>;

namespace ComparableGenerator.UnitTest
{
    // ReSharper disable once UnusedMember.Global
    public class SourceAnalyzerTest : UnitTestBase
    {
        public override async Task Should_not_be_generated_When_Comparable_and_CompareBy_is_undefined(string source)
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

        public override async Task Should_be_error_When_Comparable_is_defined_and_CompareBy_is_undefined_for_class(string source)
        {
            await source.CreateAnalyzer()
                .Should().Contain(SourceAnalyzer.CompareByIsNotDefined.Rule)
                    .WithLocation(7, 18)
                    .WithArguments("MyNamespace", "MyClass")
                .VerifyAnalyzerAsync();
        }

        public override async Task Should_be_error_When_Comparable_is_defined_and_CompareBy_is_undefined_for_struct(string source)
        {
            await source.CreateAnalyzer()
                .Should().Contain(SourceAnalyzer.CompareByIsNotDefined.Rule)
                .WithLocation(7, 19)
                .WithArguments("MyNamespace", "MyClass")
                .VerifyAnalyzerAsync();
        }

        public override async Task Should_be_error_When_Comparable_is_undefined_and_CompareBy_is_defined_for_class(string source)
        {
            #region FixedCode
            var fixedCode = @"
using ComparableGenerator;

namespace MyNamespace
{
    [Comparable]
    public class MyClass
    {
        [CompareBy]
        public int Value { get; set; }
    }
}
";
            #endregion
            await source.CreateAnalyzer()
                .Should().Contain(SourceAnalyzer.ComparableIsNotDefined.Rule)
                    .WithLocation(6, 18)
                    .WithArguments("MyNamespace", "MyClass")
                    .WithCodeFix(fixedCode)
                .VerifyCodeFixAsync();
        }

        public override async Task Should_be_error_When_Comparable_is_undefined_and_CompareBy_is_defined_for_struct(string source)
        {
            #region FixedCode
            var fixedCode = @"
using ComparableGenerator;

namespace MyNamespace
{
    [Comparable]
    public struct MyClass
    {
        [CompareBy]
        public int Value { get; set; }
    }
}
";
            #endregion
            await source.CreateAnalyzer()
                .Should().Contain(SourceAnalyzer.ComparableIsNotDefined.Rule)
                .WithLocation(6, 19)
                .WithArguments("MyNamespace", "MyClass")
                .WithCodeFix(fixedCode)
                .VerifyCodeFixAsync();
        }
    }
}
