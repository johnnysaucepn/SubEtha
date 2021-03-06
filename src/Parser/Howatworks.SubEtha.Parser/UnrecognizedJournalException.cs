﻿using System;

namespace Howatworks.SubEtha.Parser
{
    [Serializable]
    public class UnrecognizedJournalException : Exception
    {
        public string EntryType { get; set; }
        public string JournalFragment { get; set; }

        public UnrecognizedJournalException()
            : base("Found unrecognized journal entry type")
        {
        }

        public UnrecognizedJournalException(string message)
            : base(message)
        {
        }

        public UnrecognizedJournalException(string entryType, string fragment)
            : this($"Found unrecognized journal entry type '{entryType}'")
        {
            EntryType = entryType;
            JournalFragment = fragment;
        }

        public UnrecognizedJournalException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}