using System.Threading.Tasks;
using ComparableGenerator.CodeAnalysis.Analyzer;
using ComparableGenerator.UnitTest.Verifiers;

namespace ComparableGenerator.UnitTest.Assertions
{
    public class BeEmptyAssertions
    {
        private readonly Analyzer _analyzer;

        public BeEmptyAssertions(Analyzer analyzer)
        {
            _analyzer = analyzer;
        }

        public async Task VerifyAnalyzerAsync()
        {
            await CSharpAnalyzerVerifier<SourceAnalyzer>.VerifyAnalyzerAsync(_analyzer.Source);
        }
    }
}