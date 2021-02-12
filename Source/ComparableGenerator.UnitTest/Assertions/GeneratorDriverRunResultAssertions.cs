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

        public Task BeGeneratedAsync(params SyntaxTree[] expected)
        {
            if (Subject.GeneratedTrees.IsEmpty)
            {
                throw new GenerateSourceException("Source has not been generated.");
            }

            if (Subject.Diagnostics.Any())
            {
                throw new GenerateSourceException("Diagnostics has been generated.");
            }

            if (Subject.GeneratedTrees.Length != expected.Length)
            {
                throw new GenerateSourceException($"Number of expected is {expected.Length}, but actual is {Subject.GeneratedTrees.Length}.");
            }

            for (var i = 0; i < expected.Length; i++)
            {
                var actual = Subject.GeneratedTrees[i];

                var diff = actual.GetChanges(expected[i]);
                if (diff.Any())
                {
                    throw new SyntaxTreesNotEqualException(expected.First(), actual);
                }

            }

            return Task.CompletedTask;
        }

        public Task BeNotGeneratedAsync()
        {
            
            if (!Subject.GeneratedTrees.IsEmpty)
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