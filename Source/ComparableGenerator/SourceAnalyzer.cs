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

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => 
            ImmutableArray.Create(
                CompareByIsNotDefined.Rule,
                ComparableIsNotDefined.Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(AnalyzeClassNode, SyntaxKind.ClassDeclaration);
            context.RegisterSyntaxNodeAction(AnalyzeClassNode, SyntaxKind.StructDeclaration);
        }

        private void AnalyzeClassNode(SyntaxNodeAnalysisContext context)
        {
            var classDeclarationSyntax = (TypeDeclarationSyntax)context.Node;

            var isDefinedCompare = classDeclarationSyntax.AttributeLists
                .SelectMany(x => x.Attributes)
                .Any(x => x.Name.ToString() is "Comparable" or "ComparableByAttribute");
            var isDefinedCompareBy = classDeclarationSyntax.Members.Any(x => x
                .AttributeLists
                .SelectMany(attribute => attribute.Attributes)
                .Any(attribute => attribute.Name.ToString() is "CompareBy" or "CompareByAttribute"));

            if ((isDefinedCompare && isDefinedCompareBy)
                || (!isDefinedCompare && !isDefinedCompareBy))
            {
                return;
            }


            if (!isDefinedCompare)
            {
                var namespaceDeclarationSyntax = (NamespaceDeclarationSyntax)classDeclarationSyntax.Parent!;
                var namespaceName = (IdentifierNameSyntax)namespaceDeclarationSyntax.Name;

                context.ReportDiagnostic(
                    Diagnostic.Create(
                        ComparableIsNotDefined.Rule,
                        classDeclarationSyntax.Identifier.GetLocation(),
                        namespaceName.Identifier.Value,
                        classDeclarationSyntax.Identifier.Value));
                return;
            }

            if (!isDefinedCompareBy)
            {
                var namespaceDeclarationSyntax = (NamespaceDeclarationSyntax)classDeclarationSyntax.Parent!;
                var namespaceName = (IdentifierNameSyntax)namespaceDeclarationSyntax.Name;

                context.ReportDiagnostic(
                    Diagnostic.Create(
                        CompareByIsNotDefined.Rule,
                        classDeclarationSyntax.Identifier.GetLocation(),
                        namespaceName.Identifier.Value,
                        classDeclarationSyntax.Identifier.Value));
            }
        }
    }
}