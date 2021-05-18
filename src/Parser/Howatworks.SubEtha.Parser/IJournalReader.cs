using System.IO;
using Howatworks.SubEtha.Journal;

namespace Howatworks.SubEtha.Parser
{
    public interface IJournalReader
    {
        FileInfo File { get; }
        JournalLogFileInfo Context { get; }
    }
}