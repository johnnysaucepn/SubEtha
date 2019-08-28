using Howatworks.SubEtha.Monitor;

namespace Howatworks.Thumb.Core
{
    public static class BatchPolicy
    {
        public static readonly IBatchPolicy OnlyOngoing = new OnlyOngoingBatchPolicy();
        public static readonly IBatchPolicy All = new AcceptAllBatchPolicy();

        private class OnlyOngoingBatchPolicy : IBatchPolicy
        {
            public bool Accepts(BatchMode mode)
            {
                return mode == BatchMode.Ongoing;
            }
        }

        private class AcceptAllBatchPolicy : IBatchPolicy
        {
            public bool Accepts(BatchMode mode)
            {
                return true;
            }
        }
    }
}