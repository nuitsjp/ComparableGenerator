﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ComparableGenerator
{
    [Generator]
    public class SourceGenerator : ISourceGenerator
    {
        private const string Category = "ComparableGenerator";
        private const string Id = "CG0001";
        private const string MessageFormat = "Define CompareByAttribute for the any property of Type '{0}.{1}'";
        private static readonly DiagnosticDescriptor NoMembersWithCompareByAttributeDefined = new(Id,
            "No members with CompareByAttribute defined",
            MessageFormat,
            Category,
            DiagnosticSeverity.Error,
            true);
        
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

                    var members =
                        targetType.Members
                            .Select(x =>
                            {
                                var compareBy = x
                                    .AttributeLists
                                    .SelectMany(attribute => attribute.Attributes)
                                    .FirstOrDefault(attribute => attribute.Name.ToString() is "CompareBy" or "CompareByAttribute");
                                return (Member: x, CompareBy: compareBy);
                            })
                            .Where(x => x.CompareBy is not null)
                            .Select(x =>
                            {
                                var compareBy = x.CompareBy;
                                var argument = compareBy?.ArgumentList?.Arguments.SingleOrDefault();
                                if (argument is null)
                                {
                                    return (Member: x.Member, Priority: 0);
                                }

                                var expression = (LiteralExpressionSyntax)argument.Expression;
                                return (Member: x.Member, Priority: (int)expression.Token.Value);
                            })
                            .OrderBy(x => x.Priority)
                            .Select(x => context.Compilation.GetSemanticModel(x.Member.SyntaxTree).GetDeclaredSymbol(x.Member))
                            .Where(x => x is not null)
                            .Cast<ISymbol>()
                            .Select(x => x.Name)
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
                        context.AddSource($"{codeTemplate.Namespace}.{codeTemplate.Name}.Partial.cs", codeTemplate.TransformText());
                    }
                    else
                    {
                        context.ReportDiagnostic(NoMembersWithCompareByAttributeDefined.CreateDiagnostic(targetType, typeSymbol));
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

    internal static class DiagnosticDescriptorExtensions
    {
        internal static Diagnostic CreateDiagnostic(this DiagnosticDescriptor descriptor, SyntaxNode syntaxNode,
            ISymbol symbol)
            => Diagnostic.Create(descriptor, syntaxNode.GetLocation(),
                symbol.ContainingNamespace.ToString(), symbol.Name);
    }
}
