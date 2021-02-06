using System.Collections.Immutable;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

// ReSharper disable once CheckNamespace
namespace ComparableGenerator.UnitTest
{
    public static class StringExtensions
    {
        public static GeneratorDriver RunGenerator(this string source, out Compilation outputCompilation,
            out ImmutableArray<Diagnostic> diagnostics)
        {
            var inputCompilation = CSharpCompilation.Create("compilation",
                new[] {CSharpSyntaxTree.ParseText(source)},
                new[] {MetadataReference.CreateFromFile(typeof(Binder).GetTypeInfo().Assembly.Location)},
                new CSharpCompilationOptions(OutputKind.ConsoleApplication));

            var generator = new SourceGenerator();
            GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);
            return driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out outputCompilation, out diagnostics);
        }
    }
}