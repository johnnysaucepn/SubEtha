using System.Collections.Generic;

namespace Howatworks.SubEtha.Bindings
{
    public interface IBindingMapper
    {
        IReadOnlyCollection<string> GetBoundButtons(params string[] devices);
        Button GetButtonBindingByName(string name);
        string GetPresetName();
    }
}