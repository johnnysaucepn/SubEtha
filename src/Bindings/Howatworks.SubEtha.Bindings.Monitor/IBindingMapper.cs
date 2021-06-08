using System;
using System.Collections.Generic;
using System.Reactive;

namespace Howatworks.SubEtha.Bindings.Monitor
{
    public interface IBindingMapper
    {
        IReadOnlyCollection<BoundButton> GetBoundButtons(params string[] devices);
        Button GetButtonBindingByName(string name);
        IObservable<Unit> BindingsChanged { get; }
    }
}