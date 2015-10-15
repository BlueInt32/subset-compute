using System;

namespace Subset
{
    internal class HiddenWhenAttribute : Attribute
    {
        public VisibilityLevel[] Levels { get; set; }
        public HiddenWhenAttribute(params VisibilityLevel[] levels)
        {
            Levels = levels;
        }
    }
}