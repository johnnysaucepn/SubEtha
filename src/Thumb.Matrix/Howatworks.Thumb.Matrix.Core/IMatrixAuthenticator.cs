using System;
using System.Threading.Tasks;

namespace Howatworks.Thumb.Matrix.Core
{
    public interface IMatrixAuthenticator
    {
        Task<bool> RequestAuthentication();
    }
}