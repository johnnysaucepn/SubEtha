using System;

namespace Howatworks.SubEtha.Bindings
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class BindingCategoryAttribute : Attribute
    {
        public BindingCategory Category { get; }
        public BindingCategoryAttribute(BindingCategory category)
        {
            Category = category;
        }
    }
}
