using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ComparableGenerator
{
    public static class ComparableGeneratorExtensions
    {
        public static IEnumerable<(MemberDeclarationSyntax Member, AttributeSyntax CompareBy, int Priority)> GetCompareByMembers(this TypeDeclarationSyntax typeDeclarationSyntax)
        {
            return typeDeclarationSyntax.Members
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
                        return (x.Member, x.CompareBy, Priority: 0);
                    }

                    var expression = (LiteralExpressionSyntax) argument.Expression;
                return (x.Member, x.CompareBy, Priority: (int) expression.Token.Value!);
                });
        }

        public static IEnumerable<(MemberDeclarationSyntax Member, AttributeSyntax CompareBy, int Priority)> GetSamePriorityMembers(
            this IEnumerable<(MemberDeclarationSyntax Member, AttributeSyntax CompareBy, int Priority)> members)
        {
            return members
                .GroupBy(x => x.Priority, x => x)
                .Where(x => 1 < x.Count())
                .SelectMany(x => x);
        }

        public static TypeSyntax GetTypeSymbol(this MemberDeclarationSyntax memberDeclarationSyntax)
        {
            if (memberDeclarationSyntax is PropertyDeclarationSyntax propertyDeclarationSyntax)
            {
                return propertyDeclarationSyntax.Type;
            }

            var fieldDeclarationSyntax = (FieldDeclarationSyntax)memberDeclarationSyntax;
            return fieldDeclarationSyntax.Declaration.Type;
        }

        public static string GetName(this MemberDeclarationSyntax memberDeclarationSyntax)
        {
            if (memberDeclarationSyntax is PropertyDeclarationSyntax propertyDeclarationSyntax)
            {
                return propertyDeclarationSyntax.Identifier.Text;
            }

            var fieldDeclarationSyntax = (FieldDeclarationSyntax)memberDeclarationSyntax;
            return fieldDeclarationSyntax.Declaration.Variables.First().Identifier.Text;
        }

        public static Location GetTypeLocation(this MemberDeclarationSyntax memberDeclarationSyntax)
        {
            if (memberDeclarationSyntax is PropertyDeclarationSyntax propertyDeclarationSyntax)
            {
                return propertyDeclarationSyntax.Type.GetLocation();
            }

            var fieldDeclarationSyntax = (FieldDeclarationSyntax)memberDeclarationSyntax;
            return fieldDeclarationSyntax.Declaration.Type.GetLocation();
        }

        public static bool IsNotImplementedIComparable(this ITypeSymbol typeSymbol)
        {
            if (typeSymbol.Name == "Object") return true;

            if (typeSymbol.Interfaces.Any(x =>
                x.ContainingNamespace.Name == "System"
                && x.Name == "IComparable")) return false;

            if (typeSymbol.GetAttributes()
                .Any(x => x.AttributeClass!.ContainingNamespace.Name == "ComparableGenerator"
                          && x.AttributeClass.Name == "ComparableAttribute")) return false;

            // If the CompareBy member is the target of code generation,
            // it is determined by looking at the attributes declared in the code.
            // Namespaces have not been determined, so if there is a better way, we will modify it.
            if (typeSymbol.GetAttributes()
                .Any(x => x.AttributeClass!.Name is "Comparable" or "ComparableAttribute")) return false;

            return typeSymbol.BaseType?.IsNotImplementedIComparable() ?? true;
        }
    }
}