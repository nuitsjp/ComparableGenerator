using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Xunit;

namespace ComparableGenerator.UnitTest
{
    public class GenerateComparableTest
    {
        [Fact]
        public async Task Should_be_generated_for_class()
        {
            var inputCompilation = CreateCompilation(@"
using System;
using ComparableGenerator;

namespace MyNamespace
{
    [Comparable]
    public partial class ClassObject
    {
        [CompareBy]
        public int Value1 { get; set; }

        [CompareBy(Priority = 2)]
        public int Value2;

        [CompareBy(Priority = 1)]
        public int Value3 { get; set; }

        public int NotApplicable { get; set; }
    }
}");

            RunGenerator(inputCompilation, out var outputCompilation, out var diagnostics);

            var outputCode = outputCompilation.SyntaxTrees.Last();
            var text = await outputCode.GetTextAsync();
            text.ToString().Should().Be(@"using System;

namespace MyNamespace
{
    public partial class ClassObject : IComparable, IComparable<ClassObject>
    {
        public int CompareTo(object other)
        {
            if (other is null) return 1;

            if (other is ClassObject concreteObject)
            {
                return CompareTo(concreteObject);
            }

            throw new ArgumentException(""Object is not a MyNamespace.ClassObject."");
        }

        public int CompareTo(ClassObject other)
        {
            if (other is null) return 1;

            int compared;

            compared = Value1.CompareTo(other.Value1);
            if (compared != 0) return compared;

            compared = Value3.CompareTo(other.Value3);
            if (compared != 0) return compared;

            return Value2.CompareTo(other.Value2);
        }
    }
}
");

            diagnostics.Should().BeEmpty();
        }

        private static Compilation CreateCompilation(string source)
            => CSharpCompilation.Create("compilation",
                new[] { CSharpSyntaxTree.ParseText(source) },
                new[] { MetadataReference.CreateFromFile(typeof(Binder).GetTypeInfo().Assembly.Location) },
                new CSharpCompilationOptions(OutputKind.ConsoleApplication));

        private GeneratorDriver RunGenerator(Compilation inputCompilation, out Compilation outputCompilation,
            out ImmutableArray<Diagnostic> diagnostics)
        {
            var generator = new SourceGenerator();
            GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);
            return driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out outputCompilation, out diagnostics);
        }
    }
}
