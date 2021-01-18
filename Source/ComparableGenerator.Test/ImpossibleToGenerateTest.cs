using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Xunit;

namespace ComparableGenerator.Test
{
    public class ImpossibleToGenerateTest
    {
        [Fact]
        public void CompareBy_is_not_defined()
        {
            var inputCompilation = CreateCompilation(@"
namespace MyNamespace
{
    [Comparable]
    public class MyClass
    {
    }
}
");

            RunGenerator(inputCompilation, out var diagnostics);

            diagnostics.Should().HaveCount(1);
            diagnostics.Single().ToString()
                .Should().Be("(4,5): error CG0001: Define CompareByAttribute for the any property of Type 'MyNamespace.MyClass'");
        }

        private static Compilation CreateCompilation(string source)
            => CSharpCompilation.Create("compilation",
                new[] { CSharpSyntaxTree.ParseText(source) },
                new[] { MetadataReference.CreateFromFile(typeof(Binder).GetTypeInfo().Assembly.Location) },
                new CSharpCompilationOptions(OutputKind.ConsoleApplication));

        private GeneratorDriver RunGenerator(Compilation inputCompilation, out ImmutableArray<Diagnostic> diagnostics)
        {
            var generator = new SourceGenerator();
            GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);
            return driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out _, out diagnostics);
        }

        private GeneratorDriver RunGenerator(Compilation inputCompilation, out Compilation outputCompilation,
            out ImmutableArray<Diagnostic> diagnostics)
        {
            var generator = new SourceGenerator();
            GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);
            return driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out outputCompilation, out diagnostics);
        }
    }
}