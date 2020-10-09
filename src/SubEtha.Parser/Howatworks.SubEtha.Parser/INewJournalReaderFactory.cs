using System.IO;

namespace Howatworks.SubEtha.Parser
{
    public interface INewJournalReaderFactory
    {
        NewLogJournalReader CreateLogJournalReader(FileInfo file);
        NewLiveJournalReader CreateLiveJournalReader(FileInfo file);
    }
}