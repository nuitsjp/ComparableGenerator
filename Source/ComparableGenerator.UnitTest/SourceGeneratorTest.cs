using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Xunit;

namespace ComparableGenerator.UnitTest
{
    public class SourceGeneratorTest : UnitTestBase
    {
        public override Task Should_not_be_generated_for_CompareAttribute_is_not_defined(string source)
        {
            RunGenerator(CreateCompilation(source), out var outputCompilation, out var diagnostics);

            diagnostics.Should().BeEmpty();
            outputCompilation.SyntaxTrees
                .Should().HaveCount(1);

            return Task.CompletedTask;
        }

        public override Task Should_be_generated_for_class(string source)
        {
            RunGenerator(CreateCompilation(source), out var outputCompilation, out var diagnostics);


            diagnostics.Should().BeEmpty();

            var expected = CSharpSyntaxTree.ParseText(@"using System;

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
            outputCompilation.SyntaxTrees
                .Should().HaveCount(2)
                .And.Subject.Last()
                    .Should().Be(expected);

            return Task.CompletedTask;
        }

        public override Task Should_be_generated_for_struct(string source)
        {
            RunGenerator(CreateCompilation(source), out var outputCompilation, out var diagnostics);

            diagnostics.Should().BeEmpty();

            var expected = CSharpSyntaxTree.ParseText(@"using System;

namespace MyNamespace
{
    public partial struct StructObject : IComparable, IComparable<StructObject>
    {
        public int CompareTo(object other)
        {
            if (other is null) return 1;

            if (other is StructObject concreteObject)
            {
                return CompareTo(concreteObject);
            }

            throw new ArgumentException(""Object is not a MyNamespace.StructObject."");
        }

        public int CompareTo(StructObject other)
        {
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

            outputCompilation.SyntaxTrees
                .Should().HaveCount(2)
                .And.Subject.Last()
                    .Should().Be(expected);

            return Task.CompletedTask;
        }

        public override Task Should_not_be_generated_When_not_exists_CompareBy(string source)
        {
            RunGenerator(CreateCompilation(source), out var outputCompilation, out var diagnostics);

            diagnostics.Should().BeEmpty();
            outputCompilation.SyntaxTrees
                .Should().HaveCount(1);

            return Task.CompletedTask;
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
