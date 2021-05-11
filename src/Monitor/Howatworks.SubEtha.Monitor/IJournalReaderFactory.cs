using System.IO;
using Howatworks.SubEtha.Parser;

namespace Howatworks.SubEtha.Monitor
{
    public interface IJournalReaderFactory
    {
        LogJournalReader CreateLogJournalReader(FileInfo file);
        LiveJournalReader CreateLiveJournalReader(FileInfo file);
    }
}