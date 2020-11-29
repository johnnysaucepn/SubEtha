using System;
using System.Threading.Tasks;

namespace Howatworks.Matrix.Core
{
    public interface IMatrixAuthenticator
    {
        Task<bool> RequestAuthentication();
    }
}