using System;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Howatworks.Thumb.Matrix.Core
{
    public class DummyAuthenticator : IMatrixAuthenticator
    {
        private bool _dummyValue;

        public DummyAuthenticator() : this(false)
        {
        }

        public DummyAuthenticator(bool shouldBeAuthenticated)
        {
            _dummyValue = shouldBeAuthenticated;
        }

        public async Task<bool> RequestAuthentication()
        {
            return await Task.Run(() => _dummyValue);
        }
    }
}