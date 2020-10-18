using System.IO;
using Howatworks.SubEtha.Parser;

namespace Howatworks.SubEtha.Monitor
{
    public interface INewJournalReaderFactory
    {
        NewLogJournalReader CreateLogJournalReader(FileInfo file);
        NewLiveJournalReader CreateLiveJournalReader(FileInfo file);
    }
}