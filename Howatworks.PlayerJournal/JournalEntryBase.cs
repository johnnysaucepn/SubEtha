using System;

namespace Howatworks.PlayerJournal
{
    public abstract class JournalEntryBase
    {
        private DateTime _timeStamp = DateTime.SpecifyKind(default(DateTime), DateTimeKind.Utc);

        public DateTime TimeStamp
        {
            get
            {
                return _timeStamp.ToUniversalTime();
            }
            set
            {
                _timeStamp = value.ToUniversalTime();
            }
        }

        public string GameVersionDiscriminator { get; set; }

        public string Event { get; set; }

    }
}
