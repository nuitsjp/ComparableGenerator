using System.Collections.Generic;

namespace ComparableGenerator
{
    public partial class CodeTemplate
    {
        public string? Namespace { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public List<string> Members { get; set; } = new();
    }
}