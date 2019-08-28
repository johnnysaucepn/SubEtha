using System;

namespace Howatworks.SubEtha.Journal
{
    /// <inheritdoc />
    /// <summary>
    /// Apply an alternative name matching the event in the log, in case
    /// the canonical name isn't representable in idiomatic C#
    /// </summary>
    public class JournalNameAttribute : Attribute
    {
        public string Name { get; }

        public JournalNameAttribute(string name)
        {
            Name = name;
        }
    }
}