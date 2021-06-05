using System;
using System.Collections.Generic;

namespace Howatworks.SubEtha.Bindings.Monitor
{
    public interface IBindingMapper
    {
        event EventHandler BindingsChanged;
        IReadOnlyCollection<BoundButton> GetBoundButtons(params string[] devices);
        Button GetButtonBindingByName(string name);
        string GetPresetName();
    }
}