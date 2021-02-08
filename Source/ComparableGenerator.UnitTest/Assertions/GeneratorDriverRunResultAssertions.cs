using System.Linq;
using System.Threading.Tasks;
using FluentAssertions.Primitives;
using Microsoft.CodeAnalysis;

namespace ComparableGenerator.UnitTest.Assertions
{
    public class GeneratorDriverRunResultAssertions : ReferenceTypeAssertions<GeneratorDriverRunResult, GeneratorDriverRunResultAssertions>
    {
        protected override string Identifier => "generatorDriverRunResult";

        public GeneratorDriverRunResultAssertions(GeneratorDriverRunResult generatorDriverRunResult) : base(generatorDriverRunResult)
        {
        }

        public Task BeGeneratedAsync(SyntaxTree expected)
        {
            if (Subject.GeneratedTrees.IsEmpty)
            {
                throw new GenerateSourceException("Source has not been generated.");
            }

            if (Subject.Diagnostics.Any())
            {
                throw new GenerateSourceException("Diagnostics has been generated.");

            }

            var actual = Subject.GeneratedTrees.Last();

            var diff = actual.GetChanges(expected);
            if (diff.Any())
            {
                throw new SyntaxTreesNotEqualException(actual, expected);
            }


            return Task.CompletedTask;
        }

        public Task BeNotGeneratedAsync()
        {
            
            if (1 < Subject.GeneratedTrees.Length)
            {
                throw new GenerateSourceException("Source has been generated.");
            }

            if (Subject.Diagnostics.Any())
            {
                throw new GenerateSourceException("Diagnostics has been generated.");

            }

            return Task.CompletedTask;
        }
    }
}