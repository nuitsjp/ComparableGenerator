﻿using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.CodeAnalysis;
using Xunit;
using VerifyCS = ComparableGenerator.UnitTest.Verifiers.CSharpAnalyzerVerifier<
    ComparableGenerator.SourceAnalyzer>;

namespace ComparableGenerator.UnitTest
{
    public class SourceAnalyzerTest : UnitTestBase
    {
        public override async Task Should_not_be_generated_for_CompareAttribute_is_not_defined(string source)
        {
            await source.CreateAnalyzer()
                .Should().NotBeGeneratedDiagnosticsAsync();
        }

        public override async Task Should_be_generated_for_class(string source)
        {
            await source.CreateAnalyzer()
                .Should().NotBeGeneratedDiagnosticsAsync();
        }

        public override async Task Should_be_generated_for_struct(string source)
        {
            await source.CreateAnalyzer()
                .Should().NotBeGeneratedDiagnosticsAsync();
        }

        public override async Task Should_not_be_generated_When_not_exists_CompareBy(string source)
        {
            var expected =
                VerifyCS
                    .Diagnostic(SourceAnalyzer.CompareByIsNotDefined.Rule)
                    .WithLocation(7, 18)
                    .WithArguments("MyNamespace", "MyClass");
            await source.CreateAnalyzer()
                .Should().BeGeneratedDiagnosticsAsync(expected);
        }
    }
}
