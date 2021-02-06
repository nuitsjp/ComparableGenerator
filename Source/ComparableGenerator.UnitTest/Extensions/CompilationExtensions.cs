using System.Linq;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using Microsoft.CodeAnalysis;
using Xunit.Sdk;

// ReSharper disable once CheckNamespace
namespace ComparableGenerator.UnitTest
{
    public static class CompilationExtensions
    {
        public static CompilationAssertions Should(this Compilation compilation)
        {
            return new CompilationAssertions(compilation);
        }

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

        public class GenerateSourceException : AssertionFailedException
        {
            public GenerateSourceException(string message) : base(message)
            {
            }
        }

        public class SyntaxTreesNotEqualException : AssertActualExpectedException
        {
            public SyntaxTreesNotEqualException(
                SyntaxTree expected,
                SyntaxTree actual)
                : base(expected, actual, "Generated SyntaxTree differs from the expected one.", "Expected SyntaxTree", "Actual SyntaxTree") { }
        }
    }
}