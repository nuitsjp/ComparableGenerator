using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace ComparableGenerator
{
    public partial class CodeTemplate
    {
        public string? Namespace { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public string? Accessibility { get; set; }
        public List<string> Members { get; set; } = new();
    }
}