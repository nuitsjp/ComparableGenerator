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
        internal const string SourceSuffix = "GeneratedByComparableGenerator";

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
            try
            {
                var receiver = context.SyntaxReceiver as SyntaxReceiver;
                if (receiver == null) return;

                foreach (var targetType in receiver.Targets)
                {
                    var typeSymbol = context.Compilation.GetSemanticModel(targetType.SyntaxTree).GetDeclaredSymbol(targetType);
                    if (typeSymbol == null) throw new Exception("can not get typeSymbol.");

                    var multipleVariablesFields = 
                        targetType
                            .Members
                            .OfType<FieldDeclarationSyntax>()
                            .Where(x => 1 < x.Declaration.Variables.Count);
                    if(multipleVariablesFields.Any()) continue;

                    var compareByMembers = targetType.GetCompareByMembers().ToArray();
                    if(compareByMembers.GetSamePriorityMembers().Any()) continue;;

                    var members =
                        compareByMembers.OrderBy(x => x.Priority)
                            .Select(x =>
                            {
                                if (x.Member is FieldDeclarationSyntax field)
                                {
                                    return field.Declaration.Variables.Single().Identifier.Text;
                                }
                                else
                                {
                                    return context.Compilation.GetSemanticModel(x.Member.SyntaxTree)
                                        .GetDeclaredSymbol(x.Member)!.Name;
                                }
                            })
                            .ToList();

                    var codeTemplate = new CodeTemplate
                    {
                        Namespace = typeSymbol.ContainingNamespace.ToDisplayString(),
                        Name = typeSymbol.Name,
                        Type = targetType is StructDeclarationSyntax ? "struct" : "class",
                        Members = members
                    };

                    if (members.Any())
                    {
                        context.AddSource($"{codeTemplate.Namespace}.{codeTemplate.Name}.{SourceSuffix}.cs", codeTemplate.TransformText());
                    }
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
