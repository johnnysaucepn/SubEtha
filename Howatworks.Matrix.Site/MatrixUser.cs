using System.Collections.Generic;
using Nancy.Security;

namespace Howatworks.Matrix.Site
{
    public class MatrixUser : IUserIdentity
    {
        public string UserName { get; private set; }

        public IEnumerable<string> Claims { get; private set; }

        public MatrixUser(string username)
        {
            UserName = username;
            Claims = new[] {"User"};
        }
    }
}
