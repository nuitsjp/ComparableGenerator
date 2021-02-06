using System.Threading.Tasks;
using ComparableGenerator.UnitTest.Verifiers;
using FluentAssertions.Primitives;
using Microsoft.CodeAnalysis.Testing;

// ReSharper disable once CheckNamespace
namespace ComparableGenerator.UnitTest
{
    public static class AnalyzerExtensions
    {
        public class Analyzer
        {
            public string Source { get; }

            public Analyzer(string source)
            {
                Source = source;
            }
        }

        public static Analyzer CreateAnalyzer(this string source) => new Analyzer(source);

        public static AnalyzerAssertions Should(this Analyzer instance)
        {
            return new AnalyzerAssertions(instance);
        }


        public class AnalyzerAssertions : ReferenceTypeAssertions<Analyzer, AnalyzerAssertions>
        {
            public AnalyzerAssertions(Analyzer instance)
            {
                Subject = instance;
            }

            protected override string Identifier => "analyzer";

            public async Task BeGeneratedDiagnosticsAsync(params DiagnosticResult[] expected)
            {
                await CSharpAnalyzerVerifier<SourceAnalyzer>.VerifyAnalyzerAsync(Subject.Source, expected);
            }

            public async Task NotBeGeneratedDiagnosticsAsync()
            {
                await CSharpAnalyzerVerifier<SourceAnalyzer>.VerifyAnalyzerAsync(Subject.Source);
            }
        }
    }
}