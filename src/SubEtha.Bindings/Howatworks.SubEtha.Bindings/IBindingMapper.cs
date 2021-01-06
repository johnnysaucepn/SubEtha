using System;
using System.Collections.Generic;

namespace Howatworks.SubEtha.Bindings
{
    public interface IBindingMapper
    {
        event EventHandler BindingsChanged;
        IReadOnlyCollection<string> GetBoundButtons(params string[] devices);
        Button GetButtonBindingByName(string name);
        string GetPresetName();
    }
}