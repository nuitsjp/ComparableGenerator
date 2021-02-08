using FluentAssertions.Primitives;
using Microsoft.CodeAnalysis;

namespace ComparableGenerator.UnitTest.Assertions
{
    public class AnalyzerAssertions : ReferenceTypeAssertions<Analyzer, AnalyzerAssertions>
    {
        public AnalyzerAssertions(Analyzer instance)
        {
            Subject = instance;
        }

        protected override string Identifier => "analyzer";

        public ContainAssertions Contain(DiagnosticDescriptor rule)
        {
            return new(Subject, rule);
        }

        public BeEmptyAssertions BeEmpty()
        {
            return new(Subject);
        }
    }
}