using System;

namespace Howatworks.SubEtha.Parser
{
    [Serializable]
    public class UnrecognizedJournalException : JournalParseException
    {
        public string EntryType { get; set; }

        public UnrecognizedJournalException()
            : base("Found unrecognized journal entry type")
        {
        }

        public UnrecognizedJournalException(string message)
            : base(message)
        {
        }

        public UnrecognizedJournalException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public UnrecognizedJournalException(string message, string fragment, Exception innerException) : base(message, fragment, innerException)
        {
        }

        public UnrecognizedJournalException(string entryType, string fragment)
            : base($"Found unrecognized journal entry type '{entryType}'", fragment)
        {
            EntryType = entryType;
        }
    }
}