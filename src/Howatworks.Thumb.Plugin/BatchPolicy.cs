using Howatworks.SubEtha.Monitor;

namespace Howatworks.Thumb.Plugin
{
    public static class BatchPolicy
    {
        public static readonly IBatchPolicy OnlyOngoing = new OnlyOngoingBatchPolicy();
        public static readonly IBatchPolicy All = new AcceptAllBatchPolicy();

        public class OnlyOngoingBatchPolicy : IBatchPolicy
        {
            public bool Accepts(BatchMode mode)
            {
                return mode == BatchMode.Ongoing;
            }
        }

        public class AcceptAllBatchPolicy : IBatchPolicy
        {
            public bool Accepts(BatchMode mode)
            {
                return true;
            }
        }
    }
}