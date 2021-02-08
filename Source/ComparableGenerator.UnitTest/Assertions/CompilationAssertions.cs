using System.Linq;
using FluentAssertions;
using FluentAssertions.Primitives;
using Microsoft.CodeAnalysis;

namespace ComparableGenerator.UnitTest.Assertions
{
    public class CompilationAssertions : ReferenceTypeAssertions<Compilation, CompilationAssertions>
    {
        public CompilationAssertions(Compilation instance)
        {
            Subject = instance;
        }

        protected override string Identifier => "syntaxTree";

        public AndConstraint<CompilationAssertions> BeGenerated(SyntaxTree expected)
        {
            if (Subject.SyntaxTrees.Count() != 2)
            {
                throw new GenerateSourceException("Source has not been generated.");
            }

            var actual = Subject.SyntaxTrees.Last();

            var diff = actual.GetChanges(expected);
            if (diff.Any())
            {
                throw new SyntaxTreesNotEqualException(actual, expected);
            }


            return new AndConstraint<CompilationAssertions>(this);
        }

        public AndConstraint<CompilationAssertions> BeNotGenerated()
        {
            if (1 < Subject.SyntaxTrees.Count())
            {
                throw new GenerateSourceException("Source has been generated.");
            }

            return new AndConstraint<CompilationAssertions>(this);
        }
    }
}