using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ComparableGenerator
{
    [Generator]
    public class SourceGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
#if DEBUG
            if (!System.Diagnostics.Debugger.IsAttached)
            {
                //System.Diagnostics.Debugger.Launch();
            }
#endif

            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
        }

        public void Execute(GeneratorExecutionContext context)
        {
            context.AddSource("ComparableAttribute.cs", new ComparableAttributeTemplate().TransformText());
            context.AddSource("CompareByAttribute.cs", new CompareByAttributeTemplate().TransformText());
            try
            {
                var receiver = context.SyntaxReceiver as SyntaxReceiver;
                if (receiver == null) return;

                foreach (var targetType in receiver.Targets)
                {
                    var typeSymbol = context.Compilation.GetSemanticModel(targetType.SyntaxTree).GetDeclaredSymbol(targetType);
                    if (typeSymbol == null) throw new Exception("can not get typeSymbol.");

                    var members =
                        targetType.Members
                            .Select(x => context.Compilation.GetSemanticModel(x.SyntaxTree).GetDeclaredSymbol(x))
                            .Where(x => x is not null)
                            .Cast<ISymbol>()
                            .Select(x => x.Name)
                            .ToList();

                    context.AddSource("StructObject.Partial.cs", new CodeTemplate
                    {
                        Namespace = typeSymbol.ContainingNamespace.ToDisplayString(),
                        Type = typeSymbol.Name,
                        Members = members
                    }.TransformText());
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine(e.ToString());
            }
        }

        class SyntaxReceiver : ISyntaxReceiver
        {
            public List<TypeDeclarationSyntax> Targets { get; } = new();

            public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
            {
                if (syntaxNode is TypeDeclarationSyntax typeDeclarationSyntax && typeDeclarationSyntax.AttributeLists.Count > 0)
                {
                    var attr = 
                        typeDeclarationSyntax
                            .AttributeLists
                            .SelectMany(x => x.Attributes)
                            .FirstOrDefault(x => x.Name.ToString() is "Comparable" or "ComparableAttribute");
                    if (attr != null)
                    {
                        Targets.Add(typeDeclarationSyntax);
                    }
                }
            }
        }
    }
}
