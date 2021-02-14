using System.Collections.Generic;
using FluentAssertions.Primitives;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;

namespace ComparableGenerator.UnitTest.Assertions
{
    public class AnalyzerAssertions : ReferenceTypeAssertions<Analyzer, AnalyzerAssertions>
    {
        private readonly List<DiagnosticResult> _diagnosticResults = new();
        public IEnumerable<DiagnosticResult> DiagnosticResults => _diagnosticResults;

        public AnalyzerAssertions(Analyzer instance)
        {
            Subject = instance;
        }

        protected override string Identifier => "analyzer";

        public void AddDiagnosticResult(DiagnosticResult diagnosticResult) => _diagnosticResults.Add(diagnosticResult);

        public ContainAssertions Contain(DiagnosticDescriptor rule)
        {
            return new(this, rule);
        }

        public BeEmptyAssertions BeEmpty()
        {
            return new(Subject);
        }
    }
}