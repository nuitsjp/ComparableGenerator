using System.Linq;
using System.Threading.Tasks;
using ComparableGenerator.UnitTest.Verifiers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;

namespace ComparableGenerator.UnitTest.Assertions
{
    public class ContainAssertions
    {
        private readonly AnalyzerAssertions _analyzerAssertions;
        private DiagnosticResult _diagnosticResult;
        private string _fixedCode;
        public ContainAssertions(AnalyzerAssertions analyzerAssertions, DiagnosticDescriptor rule)
        {
            _analyzerAssertions = analyzerAssertions;
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

        public AnalyzerAssertions And()
        {
            _analyzerAssertions.AddDiagnosticResult(_diagnosticResult);
            return _analyzerAssertions;
        }

        public async Task VerifyAnalyzerAsync()
        {
            _analyzerAssertions.AddDiagnosticResult(_diagnosticResult);
            await CSharpAnalyzerVerifier<SourceAnalyzer>.VerifyAnalyzerAsync(_analyzerAssertions.Subject.Source, _analyzerAssertions.DiagnosticResults.ToArray());
        }

        public async Task VerifyCodeFixAsync()
        {
            _analyzerAssertions.AddDiagnosticResult(_diagnosticResult);
            await CSharpCodeFixVerifier<SourceAnalyzer, SourceFixProvider>.VerifyCodeFixAsync(_analyzerAssertions.Subject.Source, _analyzerAssertions.DiagnosticResults.ToArray(), _fixedCode);
        }

    }
}