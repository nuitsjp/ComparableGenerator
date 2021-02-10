using System.Threading.Tasks;
using ComparableGenerator.UnitTest.Verifiers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;

namespace ComparableGenerator.UnitTest.Assertions
{
    public class ContainAssertions
    {
        private readonly Analyzer _analyzer;
        private DiagnosticResult _diagnosticResult;
        private string _fixedCode;
        public ContainAssertions(Analyzer analyzer, DiagnosticDescriptor rule)
        {
            _analyzer = analyzer;
            _diagnosticResult = CSharpAnalyzerVerifier<SourceAnalyzer>.Diagnostic(rule);
        }

        public ContainAssertions WithLocation(int line, int column)
        {
            _diagnosticResult = _diagnosticResult.WithLocation(line, column);
            return this;
        }

        public ContainAssertions WithArguments(params object[] arguments)
        {
            _diagnosticResult = _diagnosticResult.WithArguments(arguments);
            return this;
        }
        public ContainAssertions WithCodeFix(string fixedCode)
        {
            _fixedCode = fixedCode;
            return this;
        }

        public async Task VerifyAnalyzerAsync()
        {
            await CSharpAnalyzerVerifier<SourceAnalyzer>.VerifyAnalyzerAsync(_analyzer.Source, _diagnosticResult);
        }

        public async Task VerifyCodeFixAsync()
        {
            await CSharpCodeFixVerifier<SourceAnalyzer, SourceFixProvider>.VerifyCodeFixAsync(_analyzer.Source, _diagnosticResult, _fixedCode);
        }

    }
}