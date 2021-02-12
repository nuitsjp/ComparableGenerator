using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ComparableGenerator
{
    public static class TypeDeclarationSyntaxExtensions
    {
        public static IEnumerable<(MemberDeclarationSyntax Member, int Priority)> GetCompareByMembers(this TypeDeclarationSyntax typeDeclarationSyntax)
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
                        return (x.Member, Priority: 0);
                    }

                    var expression = (LiteralExpressionSyntax) argument.Expression;
                    return (x.Member, Priority: (int) expression.Token.Value!);
                });
        }

        public static IEnumerable<MemberDeclarationSyntax> GetSamePriorityMembers(
            this IEnumerable<(MemberDeclarationSyntax Member, int Priority)> members)
        {
            return members
                .GroupBy(x => x.Priority, x => x.Member)
                .Where(x => 1 < x.Count())
                .SelectMany(x => x);;
        }
    }
}