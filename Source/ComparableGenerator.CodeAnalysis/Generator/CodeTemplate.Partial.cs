using System.Collections.Generic;

namespace ComparableGenerator.CodeAnalysis.Generator
{
    public partial class CodeTemplate
    {
        public CodeTemplate(string ns, string name, string type, string accessibility, List<string> members)
        {
            Namespace = ns;
            Name = name;
            Type = type;
            Accessibility = accessibility;
            Members = members;
        }

        public string Namespace { get; }
        public string Name { get; }
        public string Type { get; }
        public string Accessibility { get; }
        public List<string> Members { get; }
    }
}