using System.Threading.Tasks;
using ComparableGenerator.CodeAnalysis.Analyzer;
using ComparableGenerator.UnitTest.Assertions;

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

        public override async Task Should_not_be_generated_When_ComparableAttribute_is_not_included(string source)
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

        public override async Task Should_be_generated_for_type_with_subclass_of_generated_code_as_member(string source)
        {
            await source.CreateAnalyzer()
                .Should().BeEmpty()
                .VerifyAnalyzerAsync();
        }

        public override async Task Should_be_generated_for_type_with_subclass_of_IComparable_as_member(string source)
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
                    .WithArguments("MyClass")
                .VerifyAnalyzerAsync();
        }

        public override async Task Should_be_error_When_Comparable_is_defined_and_CompareBy_is_undefined_for_struct(string source)
        {
            await source.CreateAnalyzer()
                .Should().Contain(SourceAnalyzer.CompareByIsNotDefined.Rule)
                    .WithLocation(7, 19)
                    .WithArguments("MyClass")
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
                    .WithArguments("MyClass")
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
                    .WithArguments("MyClass")
                    .WithCodeFix(fixedCode)
                .VerifyCodeFixAsync();
        }

        public override async Task Should_be_error_When_CompareBy_with_same_priority_is_defined_for_class(string source)
        {
            await source.CreateAnalyzer()
                .Should().Contain(SourceAnalyzer.MemberWithSamePriority.Rule)
                    .WithLocation(9, 10)
                    .WithArguments("Value1")
                .And().Contain(SourceAnalyzer.MemberWithSamePriority.Rule)
                    .WithLocation(12, 10)
                    .WithArguments("Value2")
                .And().Contain(SourceAnalyzer.MemberWithSamePriority.Rule)
                    .WithLocation(17, 10)
                    .WithArguments("Value4")
                .And().Contain(SourceAnalyzer.MemberWithSamePriority.Rule)
                    .WithLocation(20, 10)
                    .WithArguments("Value5")
                .VerifyAnalyzerAsync();
        }

        public override async Task Should_be_error_When_CompareBy_with_same_priority_is_defined_for_struct(string source)
        {
            await source.CreateAnalyzer()
                .Should().Contain(SourceAnalyzer.MemberWithSamePriority.Rule)
                    .WithLocation(9, 10)
                    .WithArguments("Value1")
                .And().Contain(SourceAnalyzer.MemberWithSamePriority.Rule)
                    .WithLocation(12, 10)
                    .WithArguments("Value2")
                .And().Contain(SourceAnalyzer.MemberWithSamePriority.Rule)
                    .WithLocation(17, 10)
                    .WithArguments("Value4")
                .And().Contain(SourceAnalyzer.MemberWithSamePriority.Rule)
                    .WithLocation(20, 10)
                    .WithArguments("Value5")
                .VerifyAnalyzerAsync();
        }

        public override async Task Should_be_error_When_CompareBy_property_does_not_implement_IComparable(string source)
        {
            await source.CreateAnalyzer()
                .Should().Contain(SourceAnalyzer.NotImplementedIComparable.Rule)
                .WithLocation(10, 16)
                .WithArguments("Value")
                .VerifyAnalyzerAsync();
        }

        public override async Task Should_be_error_When_CompareBy_field_does_not_implement_IComparable(string source)
        {
            await source.CreateAnalyzer()
                .Should().Contain(SourceAnalyzer.NotImplementedIComparable.Rule)
                .WithLocation(10, 16)
                .WithArguments("Value")
                .VerifyAnalyzerAsync();
        }

        public override async Task Should_be_error_When_multiple_variables_field(string source)
        {
            await source.CreateAnalyzer()
                .Should().Contain(SourceAnalyzer.MemberWithSamePriority.Rule)
                .WithLocation(10, 20)
                .WithArguments("Value1")
                .VerifyAnalyzerAsync();
        }
    }
}
