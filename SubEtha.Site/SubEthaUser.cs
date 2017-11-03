using System.Collections.Generic;
using Nancy.Security;

namespace SubEtha.Site
{
    public class SubEthaUser : IUserIdentity
    {
        public string UserName { get; private set; }

        public IEnumerable<string> Claims { get; private set; }

        public SubEthaUser(string username)
        {
            UserName = username;
            Claims = new[] {"User"};
        }
    }
}
