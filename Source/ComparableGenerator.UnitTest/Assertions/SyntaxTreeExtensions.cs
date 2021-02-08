using System.Linq;
using FluentAssertions;
using FluentAssertions.Primitives;
using Microsoft.CodeAnalysis;

namespace ComparableGenerator.UnitTest.Assertions
{
    public static class SyntaxTreeExtensions
    {
        public static SyntaxTreeAssertions Should(this SyntaxTree instance)
        {
            return new SyntaxTreeAssertions(instance);
        }
    }
    public class SyntaxTreeAssertions : ReferenceTypeAssertions<SyntaxTree, SyntaxTreeAssertions>
    {
        public SyntaxTreeAssertions(SyntaxTree instance)
        {
            Subject = instance;
        }

        protected override string Identifier => "syntaxTree";

        public AndConstraint<SyntaxTreeAssertions> Be(SyntaxTree syntaxTree)
        {
            var diff = Subject.GetChanges(syntaxTree);
            if (diff.Any())
            {
                throw new SyntaxTreesNotEqualException(Subject, syntaxTree);
            }


            return new AndConstraint<SyntaxTreeAssertions>(this);
        }
    }
}