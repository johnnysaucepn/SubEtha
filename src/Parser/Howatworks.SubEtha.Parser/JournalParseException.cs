using System;

namespace Howatworks.SubEtha.Parser
{
    [Serializable]
    public class JournalParseException : Exception
    {
        public string JournalFragment { get; }

        public JournalParseException()
        {
        }

        public JournalParseException(string message)
            : base(message)
        {
        }

        public JournalParseException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public JournalParseException(string message, string fragment, Exception innerException)
            : base(message, innerException)
        {
            JournalFragment = fragment;
        }

        public JournalParseException(string message, string fragment)
            : base(message)
        {
            JournalFragment = fragment;
        }
    }
}
