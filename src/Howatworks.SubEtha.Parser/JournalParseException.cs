using System;

namespace Howatworks.SubEtha.Parser
{
    [Serializable]
    public class JournalParseException : Exception
    {
        public string JournalFragment { get; private set; }

        public JournalParseException(string message, string fragment, Exception innerException)
            : base(message, innerException)
        {
            JournalFragment = fragment;
        }
    }
}
