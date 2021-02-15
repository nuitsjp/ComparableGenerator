using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ComparableGenerator
{
    public static class ComparableGeneratorExtensions
    {
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
    }
}