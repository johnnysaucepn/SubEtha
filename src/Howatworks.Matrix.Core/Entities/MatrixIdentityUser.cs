using Microsoft.AspNetCore.Identity;

namespace Howatworks.Matrix.Core.Entities
{
    public class MatrixIdentityUser : IdentityUser
    {
        public MatrixIdentityUser()
        {
        }

        public MatrixIdentityUser(string username)
            : base(username)
        {
        }
    }
}
