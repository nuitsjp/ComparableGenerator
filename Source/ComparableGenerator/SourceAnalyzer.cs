using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace ComparableGenerator
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class SourceAnalyzer : DiagnosticAnalyzer
    {
        public class CompareByIsNotDefined
        {
            public const string DiagnosticId = "CG0001";

            private static readonly LocalizableString Title = new LocalizableResourceString(nameof(AnalyzerResources.TitleWhereCompareByIsNotDefined), AnalyzerResources.ResourceManager, typeof(AnalyzerResources));
            private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(AnalyzerResources.MessageFormatWhereCompareByIsNotDefined), AnalyzerResources.ResourceManager, typeof(AnalyzerResources));
            private static readonly LocalizableString Description = new LocalizableResourceString(nameof(AnalyzerResources.DescriptionWhereCompareByIsNotDefined), AnalyzerResources.ResourceManager, typeof(AnalyzerResources));
            private const string Category = "Usege";

            public static readonly DiagnosticDescriptor Rule = new(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Error, true, Description);
        }

        public class ComparableIsNotDefined
        {
            public const string DiagnosticId = "CG0002";

            private static readonly LocalizableString Title = new LocalizableResourceString(nameof(AnalyzerResources.TitleWhereComparableIsNotDefined), AnalyzerResources.ResourceManager, typeof(AnalyzerResources));
            private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(AnalyzerResources.TitleWhereComparableIsNotDefined), AnalyzerResources.ResourceManager, typeof(AnalyzerResources));
            private static readonly LocalizableString Description = new LocalizableResourceString(nameof(AnalyzerResources.TitleWhereComparableIsNotDefined), AnalyzerResources.ResourceManager, typeof(AnalyzerResources));
            private const string Category = "Usege";

            public static readonly DiagnosticDescriptor Rule = new(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Error, true, Description);
        }

        public class MemberWithSamePriority
        {
            public const string DiagnosticId = "CG0003";

            private static readonly LocalizableString Title = new LocalizableResourceString(nameof(AnalyzerResources.TitleWhereMemberWithSamePriority), AnalyzerResources.ResourceManager, typeof(AnalyzerResources));
            private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(AnalyzerResources.TitleWhereMemberWithSamePriority), AnalyzerResources.ResourceManager, typeof(AnalyzerResources));
            private static readonly LocalizableString Description = new LocalizableResourceString(nameof(AnalyzerResources.TitleWhereMemberWithSamePriority), AnalyzerResources.ResourceManager, typeof(AnalyzerResources));
            private const string Category = "Usege";

            public static readonly DiagnosticDescriptor Rule = new(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Error, true, Description);
        }

        public class NotImplementedIComparable
        {
            public const string DiagnosticId = "CG0004";

            private static readonly LocalizableString Title = new LocalizableResourceString(nameof(AnalyzerResources.TitleWhereNotImplementedIComparabl), AnalyzerResources.ResourceManager, typeof(AnalyzerResources));
            private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(AnalyzerResources.TitleWhereNotImplementedIComparabl), AnalyzerResources.ResourceManager, typeof(AnalyzerResources));
            private static readonly LocalizableString Description = new LocalizableResourceString(nameof(AnalyzerResources.DescriptionWhereNotImplementedIComparable), AnalyzerResources.ResourceManager, typeof(AnalyzerResources));
            private const string Category = "Usege";

            public static readonly DiagnosticDescriptor Rule = new(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Error, true, Description);
        }

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => 
            ImmutableArray.Create(
                CompareByIsNotDefined.Rule,
                ComparableIsNotDefined.Rule,
                MemberWithSamePriority.Rule,
                NotImplementedIComparable.Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(AnalyzeClassNode, SyntaxKind.ClassDeclaration);
            context.RegisterSyntaxNodeAction(AnalyzeClassNode, SyntaxKind.StructDeclaration);
            context.RegisterSyntaxNodeAction(AnalyzeMember, SyntaxKind.PropertyDeclaration);
            context.RegisterSyntaxNodeAction(AnalyzeMember, SyntaxKind.FieldDeclaration);
        }

        private void AnalyzeMember(SyntaxNodeAnalysisContext context)
        {
            var memberDeclarationSyntax = (MemberDeclarationSyntax)context.Node;
            var typeInfo = context.SemanticModel.GetTypeInfo(memberDeclarationSyntax.GetTypeSymbol())!;
            if (typeInfo.Type!.IsNotImplementedIComparable())
            {
                context.ReportDiagnostic(
                    Diagnostic.Create(
                        NotImplementedIComparable.Rule,
                        memberDeclarationSyntax.GetTypeLocation(),
                        memberDeclarationSyntax.GetName()));
            }
        }

        private void AnalyzeClassNode(SyntaxNodeAnalysisContext context)
        {
            var typeDeclarationSyntax = (TypeDeclarationSyntax)context.Node;

            var isDefinedComparable = typeDeclarationSyntax.AttributeLists
                .SelectMany(x => x.Attributes)
                .Any(x => x.Name.ToString() is "Comparable" or "ComparableByAttribute");
            var compareByMembers = typeDeclarationSyntax
                .GetCompareByMembers()
                .ToList();
            var isDefinedCompareBy = compareByMembers.Any();

            if (!isDefinedComparable && isDefinedCompareBy)
            {
                var namespaceDeclarationSyntax = (NamespaceDeclarationSyntax)typeDeclarationSyntax.Parent!;
                var namespaceName = (IdentifierNameSyntax)namespaceDeclarationSyntax.Name;

                context.ReportDiagnostic(
                    Diagnostic.Create(
                        ComparableIsNotDefined.Rule,
                        typeDeclarationSyntax.Identifier.GetLocation(),
                        namespaceName.Identifier.Value,
                        typeDeclarationSyntax.Identifier.Value));
                return;
            }

            if (isDefinedComparable && !isDefinedCompareBy)
            {
                var namespaceDeclarationSyntax = (NamespaceDeclarationSyntax)typeDeclarationSyntax.Parent!;
                var namespaceName = (IdentifierNameSyntax)namespaceDeclarationSyntax.Name;

                context.ReportDiagnostic(
                    Diagnostic.Create(
                        CompareByIsNotDefined.Rule,
                        typeDeclarationSyntax.Identifier.GetLocation(),
                        namespaceName.Identifier.Value,
                        typeDeclarationSyntax.Identifier.Value));
            }

            var membersWithSamePriority = compareByMembers.GetSamePriorityMembers();
            foreach (var memberDeclarationSyntax in membersWithSamePriority)
            {
                var namespaceDeclarationSyntax = (NamespaceDeclarationSyntax)typeDeclarationSyntax.Parent!;
                var namespaceName = (IdentifierNameSyntax)namespaceDeclarationSyntax.Name;

                context.ReportDiagnostic(
                    Diagnostic.Create(
                        MemberWithSamePriority.Rule,
                        memberDeclarationSyntax.GetLocation(),
                        namespaceName.Identifier.Value,
                        typeDeclarationSyntax.Identifier.Value));
            }
        }
    }

    public static class TypeSymbolExtensions
    {
        public static bool IsNotImplementedIComparable(this ITypeSymbol typeSymbol)
        {
            return !typeSymbol.Interfaces.Any(x =>
                x.ContainingNamespace.Name == "System"
                && x.Name == "IComparable");
        }
    }
}